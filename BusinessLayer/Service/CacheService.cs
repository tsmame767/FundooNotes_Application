using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CacheService:ICacheService
    {
        private readonly ICacheServiceRL cacheServiceRL;
        public CacheService(ICacheServiceRL _cacheServiceRL)
        {
            this.cacheServiceRL = _cacheServiceRL;
        }

        public List<Note> GetData<T>(string key)
        {
            return this.cacheServiceRL.GetData<T>(key);
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            return this.cacheServiceRL.SetData<T>(key, value, expirationTime);
        }
        public bool RemoveData(string key)
        {
            return this.cacheServiceRL.RemoveData(key);
        }
    }
}
