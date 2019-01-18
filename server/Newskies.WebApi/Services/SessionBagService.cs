using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Newskies.WebApi.Services
{
    public interface ISessionBagService
    {
/// <summary>
        /// Indicates whether or not a user session has been initialised. A non-null 
        /// value indicates the user session has been initialised.
        /// </summary>
        Task<bool> Initialised();

        /// <summary>
        /// Get the unique Navitaire API signature for use by an individual user session.
        /// </summary>
        Task<string> Signature();

        /// <summary>
        /// Set the unique Navitaire API signature for use by an individual user session.
        /// </summary>
        Task SetSignature(string value);

        /// <summary>
        /// Get the agent name of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task<string> AgentName();

        /// <summary>
        /// Set the agent name of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task SetAgentName(string value);

        /// <summary>
        /// Get the agent password of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task<string> AgentPassword();

        /// <summary>
        /// Set the agent password of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task SetAgentPassword(string value);

        /// <summary>
        /// Get the agent ID of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task<long> AgentId();

        /// <summary>
        /// Set the agent ID of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task SetAgentId(long value);

        /// <summary>
        /// Get the person ID of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task<long> PersonId();

        /// <summary>
        /// Set the person ID of the current user, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task SetPersonId(long value);

        /// <summary>
        /// Get the Customer Number of the current agent's person, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task<string> CustomerNumber();

        /// <summary>
        /// Set the Customer Number of the current agent's person, populated after the user (Agent or Member) 
        /// has explicitly logged in.
        /// </summary>
        Task SetCustomerNumber(string value);

        /// <summary>
        /// Get the role code that the current user is using. This may be the role code assigned 
        /// to the system web anonymous agent (where user has not explicitly logged in) or the 
        /// role code of the user (Agent or Member) after explicitly logging in.
        /// </summary>
        Task<string> RoleCode();

        /// <summary>
        /// Set the role code that the current user is using. This may be the role code assigned 
        /// to the system web anonymous agent (where user has not explicitly logged in) or the 
        /// role code of the user (Agent or Member) after explicitly logging in.
        /// </summary>
        Task SetRoleCode(string value);

        /// <summary>
        /// Get the organization code of current user's organization. This may be the web anonymous agent's
        /// organization code (where user has not explicitly logged in) or the organization code of 
        /// the user (Agent or Member) after explicitly logging in.
        /// </summary>
        Task<string> OrganizationCode();

        /// <summary>
        /// Get the organization code of current user's organization. This may be the web anonymous agent's
        /// organization code (where user has not explicitly logged in) or the organization code of 
        /// the user (Agent or Member) after explicitly logging in.
        /// </summary>
        Task SetOrganizationCode(string value);
        
        /// <summary>
        /// Get the culture code that the current user is using. This may be the role code assigned to
        /// the system web anonymous agent (where user has not explicitly logged in) or the 
        /// role code of the user (Agent or Member) after explicitly logging in.
        /// </summary>
        Task<string> CultureCode();

        /// <summary>
        /// Set the culture code that the current user is using. This may be the role code assigned to
        /// the system web anonymous agent (where user has not explicitly logged in) or the 
        /// role code of the user (Agent or Member) after explicitly logging in.
        /// </summary>
        Task SetCultureCode(string value);

        /// <summary>
        /// Get a local copy of the Booking State from Navitaire's end. 
        /// (This is synchronised when calls to BookingService.GetSessionBooking()
        /// are made.)
        /// </summary>
        Task<Booking> Booking();

        /// <summary>
        /// Set a local copy of the Booking State from Navitaire's end. 
        /// (This is synchronised when calls to BookingService.GetSessionBooking()
        /// are made.)
        /// </summary>
        Task SetBooking(Booking value);
        
        /// <summary>
        /// Get a copy of the latest seat availability request executed.
        /// </summary>
        Task<Contracts.GetSeatAvailabilityResponse> SeatAvailabilityResponse(int journeyIndex, int segmentIndex, int legIndex);

        /// <summary>
        /// Set a copy of the latest seat availability request executed.
        /// </summary>
        Task SetSeatAvailabilityResponse(Contracts.GetSeatAvailabilityResponse getSeatAvailabilityResponse, int journeyIndex, int segmentIndex, int legIndex);

        /// <summary>
        /// Gets a copy of a Dictionary of custom session string values.
        /// </summary>
        Task<Dictionary<string,string>> CustomSessionValues();

        /// <summary>
        /// Set a copy of a Dictionary of custom session string values.
        /// </summary>
        Task SetCustomSessionValues(Dictionary<string, string> value);

        /// <summary>
        /// Gets flag indicating whether or not this.Booking is "in sync" or "the same" as the
        /// Booking State on Navitaire's side. 
        /// </summary>
        Task<bool> BookingStateInSync();

        /// <summary>
        /// Sets the flag to indicate that this.Booking is "not in sync" or "not the same" as the
        /// Booking State on Navitaire's side. 
        /// </summary>
        Task SetBookingStateNotInSync();

        /// <summary>
        /// Gets the result object of last GetPostCommitResults nav call made.
        /// </summary>
        Task<Contracts.GetPostCommitResultsResponse> PostCommitResultsResponse();

        /// <summary>
        /// Sets the result object of last GetPostCommitResults nav call made.
        /// </summary>
        Task SetPostCommitResultsResponse(Contracts.GetPostCommitResultsResponse value);

        /// <summary>
        /// Gets the result object of last GetPostCommitResults nav call made.
        /// </summary>
        Task<List<string>> PostCommitBookingComments();

        /// <summary>
        /// Sets the result object of last GetPostCommitResults nav call made.
        /// </summary>
        Task SetPostCommitBookingComments(List<string> value);

        /// <summary>
        /// Initialises defaults for a user session.
        /// </summary>
        Task Initialise();

        /// <summary>
        /// Clears a user session.
        /// </summary>
        /// <param name="reInitialise">Option to re-initialise the user session.</param>
        Task Clear(bool reInitialise = false);
    }

    public class SessionBagService : ServiceBase, ISessionBagService
    {
        private const string InitialisedKey = "Initialised";
        private const string SignatureKey = "NewskiesSignature";
        private const string AgentNameKey = "NewskiesAgentName";
        private const string AgentIdKey = "NewskiesAgentId";
        private const string AgentPasswordKey = "NewskiesAgentPassword";
        private const string PersonIdKey = "NewskiesPersonId";
        private const string CustomerNumberKey = "NewskiesCustomerNumber";
        private const string RoleCodeKey = "NewskiesRoleCode";
        private const string OrganizationCodeKey = "NewskiesOrganizationCode";
        private const string CultureCodeKey = "NewskiesCultureCode";
        private const string BookingKey = "DtoBooking";
        private const string GetSeatAvailabilityResponseKey = "DtoGetSeatAvailabilityResponse";
        private const string CustomSessionValuesKey = "CustomSessionValues";
        private const string PostCommitBookingCommentsKey = "PostCommitBookingComments";
        private const string BookingStateInSyncKey = "BookingStateInSync";
        private const string PostCommitResultsResponseKey = "PostCommitResultsResponse";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionBagService(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
            : base(appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Initialise()
        {
            //await _httpContextAccessor.HttpContext.Session.LoadAsync();
            await SetInitialised(1);
            await SetRoleCode(_newskiesSettings.AnonymousAgentRole);
            await SetOrganizationCode(_newskiesSettings.AnonymousAgentOrganizationCode);
            await SetAgentId(0);
            await SetPersonId(0);
            await SetCustomerNumber(null);
            await SetAgentPassword(null);
            await SetCultureCode(_newskiesSettings.DefaultCulture);
        }

        public async Task<bool> Initialised()
        {
            var value = await GetInt32(InitialisedKey);
            return value != null;
        }

        public async Task SetInitialised(int? value)
        {
            await StoreInt32(value, InitialisedKey);
        }

        public async Task Clear(bool reInitialise = false)
        {
            await _httpContextAccessor.HttpContext.Session.LoadAsync();
            _httpContextAccessor.HttpContext.Session.Clear();
            if (reInitialise)
                await Initialise();
        }
        
        public async Task<string> Signature()
        {
            return await GetString(SignatureKey);
        }

        public async Task SetSignature(string value)
        {
            await StoreString(value, SignatureKey);
        }

        public async Task<string> AgentName()
        {
            return await GetString(AgentNameKey);
        }

        public async Task SetAgentName(string value)
        {
            await StoreString(value, AgentNameKey);
        }

        public async Task<string> AgentPassword()
        {
            return await GetString(AgentPasswordKey);
        }

        public async Task SetAgentPassword(string value)
        {
            await StoreString(value, AgentPasswordKey);
        }

        public async Task<long> AgentId()
        {
            return await GetInt64(AgentIdKey);
        }

        public async Task SetAgentId(long value)
        {
            await StoreInt64(value, AgentIdKey);
        }

        public async Task<long> PersonId()
        {
            return await GetInt64(PersonIdKey);
        }

        public async Task SetPersonId(long value)
        {
            await StoreInt64(value, PersonIdKey);
        }

        public async Task<string> CustomerNumber()
        {
            return await GetString(CustomerNumberKey);
        }

        public async Task SetCustomerNumber(string value)
        {
            await StoreString(value, CustomerNumberKey);
        }

        public async Task<string> RoleCode()
        {
            return await GetString(RoleCodeKey);
        }

        public async Task SetRoleCode(string value)
        {
            await StoreString(value, RoleCodeKey);
        }

        public async Task<string> OrganizationCode()
        {
            return await GetString(OrganizationCodeKey);
        }

        public async Task SetOrganizationCode(string value)
        {
            await StoreString(value, OrganizationCodeKey);
        }

        public async Task<string> CultureCode()
        {
            return await GetString(CultureCodeKey);
        }

        public async Task SetCultureCode(string value)
        {
            await StoreString(value, CultureCodeKey);
        }

        public async Task<Booking> Booking()
        {
            return await GetObject<Booking>(BookingKey);
        }

        public async Task SetBooking(Booking value)
        {
            await StoreObject(value, BookingKey);
            await StoreString(true.ToString(), BookingStateInSyncKey);
        }

        public async Task<Contracts.GetPostCommitResultsResponse> PostCommitResultsResponse()
        {
            return await GetObject<Contracts.GetPostCommitResultsResponse>(PostCommitResultsResponseKey);
        }

        public async Task SetPostCommitResultsResponse(Contracts.GetPostCommitResultsResponse value)
        {
            await StoreObject(value, PostCommitResultsResponseKey);
        }

        public async Task<bool> BookingStateInSync()
        {
            return await GetString(BookingStateInSyncKey) == true.ToString();
        }

        public async Task SetBookingStateNotInSync()
        {
            await StoreString(false.ToString(), BookingStateInSyncKey);
        }

        public async Task<Contracts.GetSeatAvailabilityResponse> SeatAvailabilityResponse(int journeyIndex, int segmentIndex, int legIndex)
        {
            return await GetObject<Contracts.GetSeatAvailabilityResponse>(string.Format("{0}{1}{2}{3}", GetSeatAvailabilityResponseKey, journeyIndex, segmentIndex, legIndex));
        }

        public async Task SetSeatAvailabilityResponse(Contracts.GetSeatAvailabilityResponse getSeatAvailabilityResponse, int journeyIndex, int segmentIndex, int legIndex)
        {
            await StoreObject(getSeatAvailabilityResponse, string.Format("{0}{1}{2}{3}", GetSeatAvailabilityResponseKey, journeyIndex, segmentIndex, legIndex));
        }

        public async Task<Dictionary<string, string>> CustomSessionValues()
        {
            return await GetObject<Dictionary<string, string>>(CustomSessionValuesKey);
        }

        public async Task SetCustomSessionValues(Dictionary<string, string> value)
        {
            await StoreObject(value, CustomSessionValuesKey);
        }

        public async Task<List<string>> PostCommitBookingComments()
        {
            return await GetObject<List<string>> (PostCommitBookingCommentsKey);
        }

        public async Task SetPostCommitBookingComments(List<string> value)
        {
            await StoreObject(value, PostCommitBookingCommentsKey);
        }


        private async Task StoreString(string value, string sessionKey)
        {
            await _httpContextAccessor.HttpContext.Session.LoadAsync();
            if (!string.IsNullOrEmpty(value))
            {
                _httpContextAccessor.HttpContext.Session.SetString(sessionKey, value);
                return;
            }
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
        }

        private async Task StoreObject(object value, string sessionKey)
        {
            await _httpContextAccessor.HttpContext.Session.LoadAsync();
            if (value != null)
            {
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson(sessionKey, value);
                return;
            }
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
        }

        private async Task StoreInt32(int? value, string sessionKey)
        {
            if (value != null && value.HasValue)
            {
                await _httpContextAccessor.HttpContext.Session.LoadAsync();
                _httpContextAccessor.HttpContext.Session.SetInt32(sessionKey, value.Value);
            }
        }

        private async Task StoreInt64(long value, string sessionKey)
        {
            if (value > 0)
            {
                await _httpContextAccessor.HttpContext.Session.LoadAsync();
                _httpContextAccessor.HttpContext.Session.SetString(sessionKey, value.ToString());
            }
        }

        private async Task<long> GetInt64(string sessionKey)
        {
            long result = 0;
            if (!string.IsNullOrEmpty(sessionKey))
            {
                await _httpContextAccessor.HttpContext.Session.LoadAsync();
                var str = _httpContextAccessor.HttpContext.Session.GetString(sessionKey);
                long.TryParse(str, out result);
            }
            return result;
        }

        private async Task<string> GetString(string sessionKey)
        {
            if (!string.IsNullOrEmpty(sessionKey))
            {
                await _httpContextAccessor.HttpContext.Session.LoadAsync();
                return _httpContextAccessor.HttpContext.Session.GetString(sessionKey);
            }
            return null;
        }

        private async Task<T> GetObject<T>(string sessionKey)
        {
            if (!string.IsNullOrEmpty(sessionKey))
            {
                await _httpContextAccessor.HttpContext.Session.LoadAsync();
                return _httpContextAccessor.HttpContext.Session.GetObjectFromJson<T>(sessionKey);
            }
            return default(T);
        }

        private async Task<int?> GetInt32(string sessionKey)
        {
            if (!string.IsNullOrEmpty(sessionKey))
            {
                await _httpContextAccessor.HttpContext.Session.LoadAsync();
                return _httpContextAccessor.HttpContext.Session.GetInt32(sessionKey);
            }
            return null;
        }
    }
}