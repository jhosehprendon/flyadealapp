using Amazon;
using Amazon.SQS;
using Microsoft.AspNetCore.Mvc.Filters;
using Newskies.WebApi.Contracts;
using Newskies.WebApi.Contracts.Enumerations;
using Newskies.WebApi.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flyadeal.Interceptors.Actions
{
    public class PasswordResetResponseInterceptor : IResponseInterceptor
    {
        public async Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string, string> settings)
        {
            var str = response as string;
            if (string.IsNullOrEmpty(str) || settings == null)
                throw new ResponseErrorException(ResponseErrorCode.InternalException, "Failed to reset password. ");
            if (!settings.ContainsKey("SQSQueueUrl") || string.IsNullOrEmpty(settings["SQSQueueUrl"])
                || !settings.ContainsKey("AccessKey") || string.IsNullOrEmpty(settings["AccessKey"])
                || !settings.ContainsKey("SecretKey") || string.IsNullOrEmpty(settings["SecretKey"])
                || !settings.ContainsKey("Region") || string.IsNullOrEmpty(settings["Region"]))
            {
                throw new ResponseErrorException(ResponseErrorCode.InternalException, "Missing SQS settings. ");
            }
            try
            {
                var client = FDAmazonSQSClient.Instance;
                if (client == null)
                {
                    var region = RegionEndpoint.GetBySystemName(settings["Region"].Trim().ToLower());
                    client = FDAmazonSQSClient.Initialize(settings["AccessKey"].Trim(), settings["SecretKey"].Trim(), region);
                }
                await client.SendMessageAsync(settings["SQSQueueUrl"].Trim(), str);
            }
            catch (Exception e)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(e, e.Message);
                throw new ResponseErrorException(ResponseErrorCode.InternalException, e.Message);
            }
            return await Task.FromResult(response);
        }
    }

    public sealed class FDAmazonSQSClient : AmazonSQSClient
    {
        private static volatile FDAmazonSQSClient instance;
        private static object syncRoot = new Object();
        private static bool initialized;
        private FDAmazonSQSClient(AmazonSQSConfig config, string accessKey, string secretKey) 
            : base(accessKey, secretKey, config) { }

        public static FDAmazonSQSClient Instance
        {
            get
            {
                return instance;
            }
        }

        public static bool IsInitialized
        {
            get
            {
                return initialized;
            }
        }

        public static FDAmazonSQSClient Initialize(string accessKey, string secretKey, RegionEndpoint regionEndpoint)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    var config = new AmazonSQSConfig
                    {
                        Timeout = new TimeSpan(0, 0, 5),
                        RegionEndpoint = regionEndpoint
                    };
                    instance = new FDAmazonSQSClient(config, accessKey, secretKey);
                    initialized = true;
                }
            }
            return instance;
        }
    }
}
