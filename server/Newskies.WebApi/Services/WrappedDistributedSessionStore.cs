using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;

namespace Newskies.WebApi.Services
{
    public class WrappedDistributedSessionStore : ISessionStore
    {
        private readonly IDistributedCache _cache;
        private readonly ILoggerFactory _loggerFactory;

        public WrappedDistributedSessionStore(IDistributedCache cache, ILoggerFactory loggerFactory)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        //public ISession Create(string sessionKey, TimeSpan idleTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        //{
        //    if (string.IsNullOrEmpty(sessionKey))
        //    {
        //        throw new ArgumentNullException(nameof(sessionKey));
        //    }

        //    if (tryEstablishSession == null)
        //    {
        //        throw new ArgumentNullException(nameof(tryEstablishSession));
        //    }

        //    return new WrappedDistributedSession(new DistributedSession(_cache, sessionKey, idleTimeout, tryEstablishSession, _loggerFactory, isNewSessionKey));
        //}

        public ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            return new WrappedDistributedSession(new DistributedSession(_cache, sessionKey, idleTimeout, ioTimeout, tryEstablishSession, _loggerFactory, isNewSessionKey));
            // throw new NotImplementedException();
        }
    }
}
