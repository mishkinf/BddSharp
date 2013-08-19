using System;
using System.Collections.Generic;

namespace BddSharp
{
    public class TestRepositories
    {
        private static Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public static void Reset()
        {
            _repositories = new Dictionary<Type, object>();
        }

        public static void AddFixtureToContext<T>(T item) where T : class
        {
            TestRepository<T> context = Get<T>();
            context.Add(item);
        }

        public static TestRepository<T> Get<T>() where T : class
        {
            // look up existing context
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (TestRepository<T>)_repositories[typeof(T)];
            }
            else
            {
                // or else create one and store the reference then return it
                TestRepository<T> context = new TestRepository<T>();
                _repositories.Add(typeof(T), context);
                return context;
            }
        }
    }
}
