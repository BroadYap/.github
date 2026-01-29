using System.Collections.Concurrent;
using BroadYap.Models;

namespace BroadYap.DataService
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connection = new();

        public ConcurrentDictionary<string, UserConnection> Connection => _connection;
    }
}