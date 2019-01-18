using Microsoft.AspNetCore.Mvc;
using Navitaire.WebServices.FaultContracts;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace Newskies.WebApi.Extensions
{
    public static class ResponseErrorExtensions
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static IActionResult ErrorActionResult(this Exception e, string controllerName = "", string actionName = "", string recordLocator = "")
        {
            if (e.GetType() == typeof(ResponseErrorException))
                if (((ResponseErrorException)e).ErrorCode == ResponseErrorCode.Unauthorised)
                    return ReturnError(HttpStatusCode.Unauthorized, e);
                else
                    return ReturnError(HttpStatusCode.BadRequest, e);

            if (e as FaultException<APIGeneralFault> != null && ((FaultException<APIGeneralFault>)e).Detail.ErrorType == "PromotionNotFound")
                return ReturnError(HttpStatusCode.BadRequest, new ResponseErrorException(
                    ResponseErrorCode.PromotionNotFound, ((FaultException<APIGeneralFault>)e).Detail.Message));

            var logStr = string.Format("[{0}/{1}][{2}] {3}",controllerName, actionName, !string.IsNullOrEmpty(recordLocator) ? recordLocator : "------", GetMessages(e).ConcatStringArray());
            if (e as FaultException<APIValidationFault> != null)
            {
                logger.Error(string.Format("[{0}]{1}", ResponseErrorCode.InternalNavApiValidationError.ToString(), logStr));
                return ReturnError(HttpStatusCode.InternalServerError, new ResponseErrorException(
                    ResponseErrorCode.InternalNavApiValidationError, GetMessages((FaultException<APIValidationFault>)e)));
            }
            if (e as FaultException<Navitaire.WebServices.DataContracts.Common.Navitaire.WebServices.FaultContracts.APIValidationFault> != null)
            {
                logger.Error(string.Format("[{0}]{1}", ResponseErrorCode.InternalNavApiValidationError.ToString(), logStr));
                return ReturnError(HttpStatusCode.InternalServerError, new ResponseErrorException(
                    ResponseErrorCode.InternalNavApiValidationError, GetMessages((FaultException<Navitaire.WebServices.DataContracts.Common.Navitaire.WebServices.FaultContracts.APIValidationFault>)e)));
            }
            if (e as FaultException<APIGeneralFault> != null ||
                e as FaultException<APICriticalFault> != null ||
                e as FaultException<APISecurityFault> != null ||
                e as FaultException<APIUnhandledServerFault> != null ||
                e as FaultException<APIWarningFault> != null)
            {
                logger.Error(string.Format("[{0}]{1}", ResponseErrorCode.InternalNavApiError.ToString(), logStr));
                return ReturnError(HttpStatusCode.InternalServerError, new ResponseErrorException(
                    ResponseErrorCode.InternalNavApiError, GetMessages(e)));
            }
            if (e as CommunicationException != null)
            {
                logger.Error(string.Format("[{0}]{1}", ResponseErrorCode.InternalCommunicationsError.ToString(), logStr));
                return ReturnError(HttpStatusCode.InternalServerError, new ResponseErrorException(
                    ResponseErrorCode.InternalCommunicationsError, GetMessages(e)));
            }
            logger.Error(string.Format("[{0}]{1}", ResponseErrorCode.InternalException.ToString(), logStr));
            return ReturnError(HttpStatusCode.InternalServerError, new ResponseErrorException(
                    ResponseErrorCode.InternalException, GetMessages(e)));
        }

        private static string[] GetMessages(FaultException<APIValidationFault> e)
        {
            var messages = new List<string>();
            foreach (var vr in e.Detail.ValidationResults)
                messages.Add(vr.FailedValidationDescription);
            return messages.ToArray();
        }

        private static string[] GetMessages(Exception e)
        {
            var templ = "{0}: {1}";
            var messages = new List<string> { string.Format(templ, e.GetType().Name, e.Message) };
            if (e.InnerException != null)
                messages.Add(string.Format(templ, e.InnerException.GetType().Name, e.InnerException.Message));
            return messages.ToArray();
        }

        private static string ConcatStringArray(this string[] strArray)
        {
            var sb = new StringBuilder();
            foreach (var str in strArray)
                sb.Append(str + " ");
            return sb.ToString();
        }

        public static JsonResult ReturnError(HttpStatusCode statusCode, object exception)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                {
                    IgnoreSerializableInterface = true
                }, 
            };
            return new JsonResult(exception, settings) { StatusCode = (int)statusCode };
        }
    }
}
