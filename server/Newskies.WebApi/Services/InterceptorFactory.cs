using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Newskies.WebApi.Services
{
    public interface IResponseInterceptor
    {
        Task<object> OnResponse(object response, ResultExecutingContext context, Dictionary<string,string> settings);
    }

    public interface IRequestInterceptor
    {
        Task<object> OnRequest(object request, ActionExecutingContext context, Dictionary<string,string> settings);
    }

    public interface IInterceptorFactory
    {
        T Create<T>(string typeName, params object[] param) where T : class;
    }

    public class InterceptorFactory : IInterceptorFactory
    {
        private readonly IMemoryCache _typeCache;
        private readonly object _sync = new object();

        public InterceptorFactory(IOptions<InterceptorsSettings> options)
        {
            foreach(var path in options.Value.Assemblies)
            {
                var fullFilePath = Path.GetFullPath(path);
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullFilePath);
            }
            _typeCache = new MemoryCache(new MemoryCacheOptions
            {

            });
        }

        public T Create<T>(string typeName, params object[] param) where T: class
        {
            var cached = _typeCache.Get(typeName) as T;
            if (cached != null)
            {
                return cached;
            }
            lock (_sync)
            {
                cached = _typeCache.Get(typeName) as T;
                if (cached == null)
                {
                    cached = Activator.CreateInstance(Type.GetType(typeName), param) as T;
                    if (cached != null)
                    {
                        _typeCache.Set(typeName, cached);
                    }
                }
                return cached;
            }
        }
    }
}
