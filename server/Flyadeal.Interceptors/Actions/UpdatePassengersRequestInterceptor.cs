using Newskies.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Newskies.WebApi.Contracts;
using System;
using System.Collections.Generic;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Extensions;
using System.Linq;
using Flyadeal.Interceptors.Yakeen4Flyadeal;
using Flyadeal.Interceptors.Helpers;
using NLog;
using System.ServiceModel;
using Microsoft.Extensions.Caching.Memory;

namespace Flyadeal.Interceptors.Actions
{
    class UpdatePassengersRequestInterceptor : IRequestInterceptor
    {
        private const string _commentPrefix = "[DocValidation]";
        private static object _syncRoot = new Object();

        public async Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string, string> settings)
        {
            var requestData = request as UpdatePassengersRequestData;
            if (requestData == null)
            {
                return await Task.FromResult(request);
            }

            var passengers = requestData.Passengers;
            if (passengers == null || passengers.Length == 0)
                return await Task.FromResult(request);

            foreach (Passenger pax in passengers)
            {
                if (pax.Infant != null)
                {
                    foreach (PassengerTravelDocument doc in pax.PassengerTravelDocuments)
                    {
                        foreach (BookingName infName in pax.Infant.Names)
                        {
                            foreach (BookingName docName in doc.Names)
                            {
                                if (infName.FirstName.Equals(docName.FirstName, StringComparison.Ordinal) && infName.LastName.Equals(docName.LastName, StringComparison.Ordinal))
                                {
                                    // HACK - Infant document is stored in the adult's travel document collection.
                                    // FD Newskies doesn't allow to have multiple travel documents for the passenger with different Genders
                                    // Therefore adult's gender is assigned to infant travel document in FlyadealServices/src/Services/PassengersTranslator.ts line:84
                                    // The Infant's gender must also match the travel document gender, therefore assigning it here.
                                    pax.Infant.Gender = doc.Gender;
                                }
                            }
                        }
                    }
                }
            }

            var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as ISessionBagService;
            if (sessionBag == null)
                return await Task.FromResult(request);
            var booking = await sessionBag.Booking();
            if (booking == null)
                return await Task.FromResult(request);

            // Yakeen validation
            await ValidateTravelDocsAtYakeen(settings, requestData, context, sessionBag, booking, passengers);

            // Allow only corporates with flex fare to update pax name
            CheckPaxNameChange(booking.RecordLocator, booking.Journeys, booking.Passengers, passengers, await sessionBag.RoleCode());

            return await Task.FromResult(requestData);
        }
        
        private async Task ValidateTravelDocsAtYakeen(Dictionary<string, string> settings, UpdatePassengersRequestData requestData, 
            ActionExecutingContext context, ISessionBagService sessionBag, Booking booking, Passenger[] passengers)
        {
            if (settings == null)
                return;
            if (!settings.ContainsKey("YakeenCheckEnabled") || string.IsNullOrEmpty(settings["YakeenCheckEnabled"]))
                return;
            bool.TryParse(settings["YakeenCheckEnabled"], out bool yakeenCheckEnabled);
            if (!yakeenCheckEnabled)
                return;
            if (!settings.ContainsKey("YakeenMaxFailures") || string.IsNullOrEmpty(settings["YakeenMaxFailures"])
                || !settings.ContainsKey("YakeenServiceUrl") || string.IsNullOrEmpty(settings["YakeenServiceUrl"])
                || !settings.ContainsKey("YakeenTimeoutSeconds") || string.IsNullOrEmpty(settings["YakeenTimeoutSeconds"])
                || !settings.ContainsKey("YakeenUsername") || string.IsNullOrEmpty(settings["YakeenUsername"])
                || !settings.ContainsKey("YakeenPassword") || string.IsNullOrEmpty(settings["YakeenPassword"])
                || !settings.ContainsKey("YakeenChargeCode") || string.IsNullOrEmpty(settings["YakeenChargeCode"])
                || !settings.ContainsKey("YakeenExcessiveTimeoutCount") || string.IsNullOrEmpty(settings["YakeenExcessiveTimeoutCount"])
                || !settings.ContainsKey("YakeenExcessiveTimeoutPeriodSeconds") || string.IsNullOrEmpty(settings["YakeenExcessiveTimeoutPeriodSeconds"])
                || !settings.ContainsKey("YakeenExcessiveTimeoutRestSeconds") || string.IsNullOrEmpty(settings["YakeenExcessiveTimeoutRestSeconds"]))
            {
                throw new ResponseErrorException(ResponseErrorCode.InternalException, "Missing document validation settings. ");
            }
            var cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

            var comments = await sessionBag.PostCommitBookingComments();
            var invalid = false;
            foreach (var pax in passengers)
            {
                if (pax.PassengerTravelDocuments != null)
                {
                    foreach (var doc in pax.PassengerTravelDocuments)
                    {
                        try
                        {
                            await ValidateTravelDoc(doc, booking, pax.PassengerInfo.Nationality, comments, sessionBag, settings, cache);
                        }
                        catch
                        {
                            invalid = true;                            
                        }
                    }
                }
            }
            if (invalid)
                throw new ResponseErrorException(ResponseErrorCode.TravelDocumentValidationFailure,
                    "Unable to validate travel document. Please try again. ");
        }

        private async Task ValidateTravelDoc(PassengerTravelDocument doc, Booking booking, string nationalityCode, List<string> comments, 
            ISessionBagService sessionBag, Dictionary<string,string> settings, IMemoryCache cache)
        {
            if (comments != null && comments.Count > 0)
            {
                var existingComment = comments.Find(
                    c => c.StartsWith(_commentPrefix) && c.Contains("DocNum=" + doc.DocNumber));
                if (!string.IsNullOrEmpty(existingComment))
                    return;
            }
            if (booking.BookingComments != null && booking.BookingComments.Length > 0)
            {
                var existingComment = booking.BookingComments.ToList().Find(
                    c => c.CommentText.StartsWith(_commentPrefix) && c.CommentText.Contains("DocNum=" + doc.DocNumber));
                if (existingComment != null)
                    return;
            }
            var isRestingFromYakeenChecks = cache.Get<bool>("Yakeen_RestingFromChecks");
            if (isRestingFromYakeenChecks)
            {
                // dont do yakeen check
                var newComment = string.Format("NotPerformed. Tries=0, DocNum={0}, DocType={1}, Reason={2}", doc.DocNumber, doc.DocTypeCode, "Excessive service timeouts");
                await StoreBookingComment(newComment, sessionBag, comments);
                return;
            }

            var yakeenCountSessionKey = string.Format("yakeenCountSession_{0}_{1}", doc.DocNumber, doc.DocTypeCode);
            var sessionStr = await sessionBag.GetCustomSessionValue(yakeenCountSessionKey) ?? "0";
            int.TryParse(sessionStr, out int validationFailureCount);
            if (!int.TryParse(settings["YakeenMaxFailures"], out int yakeenMaxFailures))
                yakeenMaxFailures = 2;
            if (validationFailureCount >= yakeenMaxFailures)
                return;
            try
            {
                var logId = await ValidateAtYakeen(doc, nationalityCode, settings["YakeenUsername"], settings["YakeenPassword"], 
                    settings["YakeenChargeCode"], settings["YakeenServiceUrl"], settings["YakeenTimeoutSeconds"]);
                if (logId == 0) // 0 means no validation was required at yakeen therefore not performed
                    return;
                var newComment = string.Format("Success. LogId={0}, DocNum={1}, DocType={2}", logId, doc.DocNumber, doc.DocTypeCode);
                await StoreBookingComment(newComment, sessionBag, comments);
            }
            catch (Exception e)
            {
                validationFailureCount++;
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Warn(string.Format("[{0}] Tries={1}, DocNum={2}, DocType={3} Reason={4}",
                    ResponseErrorCode.TravelDocumentValidationFailure.ToString(), validationFailureCount, doc.DocNumber, doc.DocTypeCode, e.Message));
                sessionBag.SetCustomSessionValue(yakeenCountSessionKey, validationFailureCount.ToString()).GetAwaiter().GetResult();
                var isTimeout = e is TimeoutException || (e.InnerException != null && e.InnerException is TimeoutException);
                if (validationFailureCount >= yakeenMaxFailures)
                {
                    var reason = isTimeout ? "Query timeout" : e.Message;
                    var newComment = string.Format("Failed. Tries={0}, DocNum={1}, DocType={2}, Reason={3}", validationFailureCount, doc.DocNumber, doc.DocTypeCode, reason);
                    await StoreBookingComment(newComment, sessionBag, comments);
                    return;
                }

                if (isTimeout)
                {
                    if (!int.TryParse(settings["YakeenExcessiveTimeoutCount"], out int excessiveTimeoutCountSetting))
                        excessiveTimeoutCountSetting = 20;
                    if (!int.TryParse(settings["YakeenExcessiveTimeoutRestSeconds"], out int excessiveTimeoutRestSecondsSetting))
                        excessiveTimeoutRestSecondsSetting = 3600;
                    if (!int.TryParse(settings["YakeenExcessiveTimeoutPeriodSeconds"], out int excessiveTimeoutPeriodSecondsSetting))
                        excessiveTimeoutPeriodSecondsSetting = 300;

                    lock (_syncRoot)
                    {
                        var timePeriodIsRunning = cache.Get<bool>("Yakeen_PeriodIsRunning");
                        var globalTimeoutCount = timePeriodIsRunning ? cache.Get<int>("Yakeen_GlobalTimeoutCount") + 1 : 1;
                        cache.Set("Yakeen_GlobalTimeoutCount", globalTimeoutCount);
                        if (timePeriodIsRunning)
                        {
                            if (globalTimeoutCount >= excessiveTimeoutCountSetting)
                            {
                                cache.Set("Yakeen_RestingFromChecks", true, new TimeSpan(0, 0, excessiveTimeoutRestSecondsSetting));
                                cache.Remove("Yakeen_GlobalTimeoutCount");
                                cache.Remove("Yakeen_PeriodIsRunning");
                            }
                        }
                        else
                        {
                            cache.Set("Yakeen_PeriodIsRunning", true, new TimeSpan(0, 0, excessiveTimeoutPeriodSecondsSetting));
                        }
                    }
                }

                throw;
            }
        }

        private async Task StoreBookingComment(string newComment, ISessionBagService sessionBag, List<string> comments)
        {
            if (comments == null)
            {
                comments = new List<string>();
            }
            comments.Add(_commentPrefix + " " + newComment);
            await sessionBag.SetPostCommitBookingComments(comments);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nationalityCode"></param>
        /// <returns>LogID returned by Yakeen validation. Returns 0 if business rules do not require validation at Yakeen to be done.</returns>
        private async Task<int> ValidateAtYakeen(PassengerTravelDocument doc, string nationalityCode, string userName, string password, string chargeCode, string serviceUrl, string timeoutSeconds)
        {
            var reference = ""; // DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var logId = 0;
            var dobStr = doc.DOB.ToString("MM-yyyy");
            var client = YakeenClient.Instance;
            if (!int.TryParse(timeoutSeconds, out int seconds))
                seconds = 5;
            client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, seconds);
            client.Endpoint.Address = new EndpointAddress(serviceUrl);

            doc.DocNumber = doc.DocNumber.Trim();
            if (nationalityCode == "SA")
            {
                if (doc.DocTypeCode == "N")
                {
                    var response = await client.getCitizenInfoByIDAsync(new citizenInfoByIDRequest
                    {
                        userName = userName,
                        password = password,
                        chargeCode = chargeCode,
                        referenceNumber = reference,
                        nin = doc.DocNumber,
                        dateOfBirth = dobStr
                    });
                    logId = response.CitizenInfoByIDResult.logId;
                }
                else if (doc.DocTypeCode == "P")
                {
                    var response = await client.getCitizenInfoByPassportAsync(new citizenInfoByPassportRequest
                    {
                        userName = userName,
                        password = password,
                        chargeCode = chargeCode,
                        referenceNumber = reference,
                        passportNumber = doc.DocNumber,
                        dateOfBirth = dobStr
                    });
                    logId = response.CitizenInfoByPassportResult.logId;
                }
            }
            else
            {
                if (doc.DocTypeCode == "P")
                {
                    var response = await client.getAlienInfoByPassportAsync(new alienInfoByPassportRequest
                    {
                        userName = userName,
                        password = password,
                        chargeCode = chargeCode,
                        referenceNumber = reference,
                        passportNumber = doc.DocNumber,
                        nationalityCode = YakeenHelper.GetYakeenCountryCode(doc.IssuedByCode)
                    });
                    logId = response.AlienInfoByPassportResult.logId;
                }
                else if (doc.DocTypeCode == "I")
                {
                    var response = await client.getAlienInfoByIqamaAsync(new alienInfoByIqamaRequest
                    {
                        userName = userName,
                        password = password,
                        chargeCode = chargeCode,
                        referenceNumber = reference,
                        iqamaNumber = doc.DocNumber,
                        dateOfBirth = dobStr
                    });
                    logId = response.AlienInfoByIqamaResult.logId;
                }
            }
            return logId;
        }

        private void CheckPaxNameChange(string recordLocator, Journey[] journeys, Passenger[] passengers, Passenger[] reqPassengers, string sessionRoleCode)
        {
            if (string.IsNullOrEmpty(recordLocator))
                return;
            if (passengers.Length != reqPassengers.Length)
                throw new ResponseErrorException(ResponseErrorCode.PassengersLengthUpdateMismatch, "Passenger update requires details for all passengers. ");
            var isCorp = sessionRoleCode == Constants.CorporateMasterRoleCode || sessionRoleCode == Constants.CorporateSubRoleCode;
            var hasFlexFare = journeys.ToList().Find(j => j.Segments.ToList().Find(s => s.Fares.ToList().Find(f => f.ProductClass == Constants.CorporateFlexProductClass) != null) != null) != null;
            for (var i = 0; i < passengers.Length; i++)
            {
                var nameIsTheSame = reqPassengers[i].Names[0].FirstName.Equals(passengers[i].Names[0].FirstName, StringComparison.Ordinal) 
                    //&& reqPassengers[i].Names[0].Title.Equals(passengers[i].Names[0].Title, StringComparison.Ordinal)
                    && reqPassengers[i].Names[0].LastName.Equals(passengers[i].Names[0].LastName, StringComparison.Ordinal);
                if (nameIsTheSame)
                    continue;
                if (isCorp && hasFlexFare)
                    continue;
                throw new ResponseErrorException(ResponseErrorCode.PassengerNameUpdateNotAllowed, "Passenger name change not allowed. ");
            }
        }
    }
}
