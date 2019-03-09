using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.Extensions.Options;
using PetStore.Api.Settings;

namespace PetStore.Api.Services.Implementations
{
    public class LiteDbService : ILiteDbService
    {
        private readonly string _dbPath;

        public LiteDbService(IOptions<AppSettings> options)
        {
            var dbPath = options?.Value?.DbPath;

            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentNullException($"Failed to obtain LiteDb path, {nameof(dbPath)} cannot be null or empty.");

            _dbPath = dbPath;
        }

        public int Insert<T>(T document) where T: new()
        {
            if (document == null)
                throw new ArgumentNullException($"{nameof(document)} cannot be null.");

            using (var db = new LiteDatabase(_dbPath))
            {
                var dbCollection = db.GetCollection<T>(typeof(T).Name);

                var result = dbCollection.Insert(document);

                return result.AsInt32;
            }
        }

        public List<T> Get<T>(Func<LiteCollection<T>, IEnumerable<T>> handler) where T: new()
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var dbCollection = db.GetCollection<T>(typeof(T).Name);

                var result = handler(dbCollection);

                return result?.ToList();
            }
        }

        public T Get<T>(Func<LiteCollection<T>, T> handler) where T : new()
        {
            using (var db = new LiteDatabase(_dbPath))
            {
                var dbCollection = db.GetCollection<T>(typeof(T).Name);

                var result = handler(dbCollection);

                return result;
            }
        }
    }
}
