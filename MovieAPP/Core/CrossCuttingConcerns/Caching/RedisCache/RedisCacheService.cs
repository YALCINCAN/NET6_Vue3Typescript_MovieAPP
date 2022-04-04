using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }


        public T Get<T>(string key)
        {
            return Get(key) is byte[] data
                 ? Deserialize<T>(data)
                 : default;
        }


        private byte[] Get(string key)
        {
            try
            {
                return _cache.Get(key);
            }
            catch
            {
                return null;
            }
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            return await GetAsync(key, token) is byte[] data
                ? Deserialize<T>(data)
                : default;
        }


        private async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            try
            {
                return await _cache.GetAsync(key, token);
            }
            catch
            {
                return null;
            }
        }

        public void Refresh(string key)
        {
            try
            {
                _cache.Refresh(key);
            }
            catch
            {
            }
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            try
            {
                await _cache.RefreshAsync(key, token);
            }
            catch
            {
            }
        }

        public void Remove(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch
            {
            }
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            try
            {
                await _cache.RemoveAsync(key, token);
            }
            catch
            {
            }
        }

        public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
        {
            Set(key, Serialize(value), slidingExpiration);
        }


        private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null)
        {
            try
            {
                _cache.Set(key, value, GetOptions(slidingExpiration));
            }
            catch
            {
            }
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            return SetAsync(key, Serialize(value), slidingExpiration, cancellationToken);
        }


        private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
        {
            try
            {
                await _cache.SetAsync(key, value, GetOptions(slidingExpiration), token);
            }
            catch
            {
            }
        }
        private byte[] Serialize<T>(T item)
        {
            return Encoding.Default.GetBytes(JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
            {
                new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() }
        }
            }));
        }


        private T Deserialize<T>(byte[] cachedData)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(cachedData));
        }


        private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
        {
            var options = new DistributedCacheEntryOptions();
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }
            else
            {
                options.SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Default expiration time of 10 minutes.
            }

            return options;
        }
    }
}
