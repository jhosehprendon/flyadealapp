using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System.Threading;

namespace Newskies.WebApi.Services
{
    public class WrappedDistributedSession : ISession
    {
        private DistributedSession _service;
        private bool _loaded;
        private string _errorStr = "Session not loaded";

        public WrappedDistributedSession(DistributedSession service)
        {
            _service = service;
        }

        public bool IsAvailable => _service.IsAvailable;

        public string Id => _service.Id;

        public IEnumerable<string> Keys => _service.Keys;

        public void Clear()
        {
            if (_loaded)
            {
                _service.Clear();
            }
            else
            {
                throw new Exception(_errorStr);
            }
        }

        // public Task CommitAsync() => _service.CommitAsync();

        public async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _service.CommitAsync(cancellationToken);
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_loaded)
            {
                return;
            }
            await _service.LoadAsync(cancellationToken);
            _loaded = true;
        }

        public void Remove(string key)
        {
            if (_loaded)
            {
                _service.Remove(key);
            }
            else
            {
                throw new Exception(_errorStr);
            }
        }

        public void Set(string key, byte[] value)
        {
            if (_loaded)
            {
                _service.Set(key, value);
            }
            else
            {
                throw new Exception(_errorStr);
            }
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_loaded)
            {
                return _service.TryGetValue(key, out value);
            }
            else
            {
                throw new Exception(_errorStr);
            }
        }
    }
}
