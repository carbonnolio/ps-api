using System;
using System.Collections.Generic;
using LiteDB;

namespace PetStore.Api.Services
{
    public interface ILiteDbService
    {
        int Insert<T>(T document) where T : new();
        List<T> Get<T>(Func<LiteCollection<T>, IEnumerable<T>> handler) where T : new();
        T Get<T>(Func<LiteCollection<T>, T> handler) where T : new();
    }
}
