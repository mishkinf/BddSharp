using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    }
}
