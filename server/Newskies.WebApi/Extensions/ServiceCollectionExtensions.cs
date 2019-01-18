using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Newtonsoft.Json.Converters;
using Newskies.WebApi.Configuration;
using Microsoft.Extensions.Options;
using System.ServiceModel;
using Navitaire.WebServices.DataContracts.Common;
using Newskies.SessionManager;
using Newskies.QueueManager;
using Newskies.PersonManager;
using Newskies.VoucherManager;
using Newskies.AccountManager;
using System.Net;
using System;

namespace Newskies.WebApi.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddNewskiesServices(this IServiceCollection services)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            services.AddSingleton<IBookingManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new BookingManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.BookingManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.BookingManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IAgentManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new AgentManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.AgentManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.AgentManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IPersonManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new PersonManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.PersonManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.PersonManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IUtilitiesManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new UtilitiesManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.UtilitiesManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.UtilitiesManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IOperationManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new OperationManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.OperationManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.OperationManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IQueueManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new QueueManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.QueueManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.QueueManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IVoucherManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new VoucherManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.VoucherManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.VoucherManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
            services.AddSingleton<IAccountManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new AccountManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.AccountManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.AccountManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });            
            services.AddSingleton<ISessionManager>(service =>
            {
                var options = service.GetService(typeof(IOptions<AppSettings>)) as IOptions<AppSettings>;
                var client = new SessionManagerClient();
                if (!string.IsNullOrEmpty(options.Value.NewskiesSettings.ServiceEndpoints.SessionManagerUrl))
                {
                    client.Endpoint.Address = new EndpointAddress(options.Value.NewskiesSettings.ServiceEndpoints.SessionManagerUrl);
                    var timeout = options.Value.NewskiesSettings.ServiceTimeoutSeconds;
                    client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(timeout);
                    client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(timeout);
                }
                return client;
            });
        }

        internal static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.Configure<InterceptorsSettings>(configuration.GetSection("InterceptorsSettings"));
            services.Configure<SwaggerSettings>(configuration.GetSection("Swagger"));
        }

        internal static void AddSession(this IServiceCollection services, IConfiguration configuration)
        {
            var sessionExpiry = configuration.GetValue<TimeSpan>("AppSettings:ApplicationSessionOptions:IdleTimeout");
            services.AddSession(Options => { Options.IdleTimeout = sessionExpiry; });
        }

        internal static void AddRedisDistributedSession(this IServiceCollection services, IConfiguration config)
        {
            var host = config.GetValue<string>("AppSettings:ApplicationSessionOptions:RedisHost");
            var name = config.GetValue<string>("AppSettings:ApplicationSessionOptions:RedisHost");
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(name))
            {
                return;
            }
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = host;
                options.InstanceName = name;
            });
        }

        /// <summary>
        /// This ensures that all application instances use the same data protection encrypt/decrypt key.
        /// </summary>
        /// <param name="services"></param>
        internal static void AddPersistentDataProtection(this IServiceCollection services, IConfiguration config)
        {
            if (config.GetValue<bool>("AppSettings:ApplicationSessionOptions:UsePersistentDataProtection"))
            {
                services.AddDataProtection()
                     .PersistKeysToFileSystem(new DirectoryInfo(@"./"))
                     .SetApplicationName("Newskies.WebApi")
                     .DisableAutomaticKeyGeneration();
            }
        }

        internal static void AddMvcAndJsonOptions(this IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = false
                    });
                });
        }
    }
}
