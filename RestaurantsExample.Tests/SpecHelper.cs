using BddSharp;
using NUnit.Framework;
using RestaurantsExample.EntityFramework;
using System.Data;
using System.Data.Entity;
using System.Reflection;

namespace RestaurantsExample.Tests
{
    [SetUpFixture]
    public class SpecHelper
    {
        public static dynamic dataFixtures;
        public static RestaurantsTestServer testServer = new RestaurantsTestServer();

        [SetUp]
        public static void BeforeAllTests()
        {
            Nav.Host = "http://localhost:44444";

            RestaurantsContext restaurantsContext = new RestaurantsContext();
            // Runs before any of the tests are run
            dataFixtures = new BddSharp.Fixtures();

            if (restaurantsContext.Database.Connection.State == ConnectionState.Open)
                restaurantsContext.Database.Connection.Close();

            Database.SetInitializer(new RestaurantsSeedInitializer(context => dataFixtures.Load(restaurantsContext, Assembly.GetExecutingAssembly()))); ;
            restaurantsContext.Database.Initialize(true);
            restaurantsContext.Database.Connection.Open();

            testServer.Spawn();
        }

        [TearDown]
        public static void AfterAllTests()
        {
            testServer.Kill();
        }
    }
}
