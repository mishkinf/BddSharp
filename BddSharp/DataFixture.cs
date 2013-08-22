using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BddSharp
{
    public class DataFixture
    {
        public static DbContext Context { get; set; }
        public static dynamic Fixtures { get; set; }

        static protected void Add<T>(String fixtureName, T fixture) where T : class 
        {
            Fixtures[fixtureName] = fixture;

            Context.Set<T>().Add(fixture);
            TestRepositories.AddFixtureToContext(fixture);
        }

        static protected void Register<T>(String fixtureName, Expression<Func<T, bool>> predicate) where T : class
        {
            T fixture = Context.Set<T>().FirstOrDefault(predicate);
            Fixtures[fixtureName] = fixture;
            TestRepositories.AddFixtureToContext(fixture);
        }
    }
}
