using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Extensions;
using Newskies.WebApi.Configuration;
using Newskies.WebApi.Services;
using Newskies.WebApi.Swagger;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.HttpOverrides;
using System.ServiceModel;

namespace Newskies.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddJsonFile($"interceptors.json", false, true)
                .AddJsonFile($"interceptors.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddConfigurations(Configuration);
            services.AddCors();
            services.AddSession(Configuration);
            services.AddTransient<ISession, WrappedDistributedSession>();
            services.AddTransient<ISessionStore, WrappedDistributedSessionStore>();
            services.AddMemoryCache();
            services.AddRedisDistributedSession(Configuration);
            services.AddResponseCaching();
            services.AddMvcAndJsonOptions();
            services.AddPersistentDataProtection(Configuration);
            services.AddResponseCompression();
            services.AddAutoMapper();
            services.AddNewskiesServices();
            services.AddScoped<ISessionBagService, SessionBagService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<IResourcesService, ResourcesService>();
            services.AddScoped<IFlightsService, FlightsService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPassengersService, PassengersService>();
            services.AddScoped<IContactsService, ContactsService>();
            services.AddScoped<ISsrsService, SsrsService>();
            services.AddScoped<ISeatsService, SeatsService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IPaymentsService, PaymentsService>();
            services.AddScoped<ICheckinService, CheckinService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBookingCommentsService, BookingCommentsService>();
            services.AddScoped<IFeeService, FeeService>();
            services.AddSingleton<IInterceptorFactory, InterceptorFactory>();
            services.AddFlyadealSwaggerGen(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, 
            IOptions<AppSettings> options)
        {
            app.UseResponseBuffering();
            app.UseResponseCaching();
            app.UseCompressionResponse();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");
            app.UseHttpsEnforcement();
            app.UseWebApiPerformanceLogging();
            app.UseReadMeRouter();
            app.UseApiRouter();
            app.UseClientAppRouter();
            app.UseSwaggerRouter();
        }
    }
}