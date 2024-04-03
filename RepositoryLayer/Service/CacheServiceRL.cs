using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class CacheServiceRL:ICacheServiceRL
    {
        private IDatabase db;
        public CacheServiceRL(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
        }

        public List<Note> GetData<T>(string key)
        {

            var value = db.StringGet(key);

            if (!value.IsNullOrEmpty)
            {
                var notes = JsonConvert.DeserializeObject<List<Note>>(value);
                return notes;
                //return JsonConvert.DeserializeObject<T>(value);
                

            }
            return null;
        }


        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime - DateTimeOffset.Now;
            var json = JsonConvert.SerializeObject(value);
            return db.StringSet(key, json, expiryTime);
        }

        public bool RemoveData(string key)
        {
            return db.KeyDelete(key);
        }
    }
}
