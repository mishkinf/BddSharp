using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using BddSharp;
using NUnit.Framework;
using RestaurantsExample.EntityFramework;
using RestaurantsExample.Models;
using RestaurantsExample.Repositories;

namespace RestaurantsExample.Tests
{
    [SetUpFixture]
    public class SpecHelper
    {
        public static dynamic dataFixtures;
        private static string serverPath = @"C:\Users\mfaustini\Documents\Visual Studio 2012\Projects\BddSharp\RestaurantsExample";
        public static RestaurantsTestServer testServer = new RestaurantsTestServer("44444", serverPath);

        [SetUp]
        public static void BeforeAllTests()
        {
            Nav.Host = "http://localhost:44444";

            RestaurantsContext restaurantsContext = RestaurantsContext.Instance;
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
