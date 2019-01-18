using Amazon;
using Flyadeal.Interceptors.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    class CommitAgentResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var requestData = response as CommitAgentResponse;
            if (requestData == null || requestData.CommitAgentResData == null || requestData.CommitAgentResData.Agent == null)
                return await Task.FromResult(response);
            var agentRoles = requestData.CommitAgentResData.Agent.AgentRoles;
            if (agentRoles != null && agentRoles.Length > 0 && agentRoles[0].RoleCode == Constants.MemberRoleCode)
            {
                // assume new member was created if current session is an anonymous session
                var sessionBag = context.HttpContext.RequestServices.GetService(typeof(ISessionBagService)) as SessionBagService;
                if (await sessionBag.RoleCode() != Constants.AnonymousAgentRoleCode)
                    return await Task.FromResult(response);

                var parameters = new List<Parameter>
                {
                    new Parameter { name = "CultureCode", value = requestData.CommitAgentResData.Person.CultureCode },
                    new Parameter { name = "Destination", value = requestData.CommitAgentResData.Person.EmailAddress }
                };
                var message = new Message
                {
                    Organization = new Organization
                    {
                        ContactTitle = requestData.CommitAgentResData.Person.Name.Title,
                        ContactFirstName = requestData.CommitAgentResData.Person.Name.FirstName,
                        ContactLastName = requestData.CommitAgentResData.Person.Name.LastName,
                        EmailAddress = requestData.CommitAgentResData.Person.EmailAddress
                    }
                };
                var obj = new MemberActivation
                {
                    parameters = parameters,
                    message = message
                };
                var jsonStr = JsonConvert.SerializeObject(obj);
                try
                {
                    var client = FDAmazonSQSClient.Instance;
                    if (client == null)
                    {
                        var region = RegionEndpoint.GetBySystemName(settings["Region"].Trim().ToLower());
                        client = FDAmazonSQSClient.Initialize(settings["AccessKey"].Trim(), settings["SecretKey"].Trim(), region);
                    }
                    await client.SendMessageAsync(settings["SQSQueueUrl"].Trim(), jsonStr);
                }
                catch (Exception e)
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Error(e, e.Message);
                    throw new ResponseErrorException(ResponseErrorCode.InternalException, e.Message);
                }
            }
            return await Task.FromResult(response);
        }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Organization
    {
        public string ContactTitle { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class Message
    {
        public Organization Organization { get; set; }
    }

    public class MemberActivation
    {
        public List<Parameter> parameters { get; set; }
        public Message message { get; set; }
    }
}
