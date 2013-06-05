using System;
using BddSharp;
using NUnit.Framework;
using RestaurantsExample.Managers;
using RestaurantsExample.Models;

namespace RestaurantsExample.Tests.Spec
{
    [TestFixture]
    public class RestaurantManagerSpec
    {
        RestaurantManager restaurantManager = new RestaurantManager();

        [SetUp]
        public void SetupRestaurantManagerSpec()
        {
            TestRepository<Restaurant> restaurantRepository = TestRepositories.Get<Restaurant>();
            restaurantManager.restaurantRepository = restaurantRepository;
        }

        [Test, Description("When given a time, should return restaurants that are open")]
        public void TestOpenRestaurants()
        {
            Restaurant[] restaurants = restaurantManager.GetOpenRestaurants(DateTime.Now);
            Assert.IsTrue(restaurants.Length == 1);
        }
    }
}
