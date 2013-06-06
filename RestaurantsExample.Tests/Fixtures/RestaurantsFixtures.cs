using BddSharp;
using RestaurantsExample.Models;
using System;
using System.Data.Entity;

namespace RestaurantsExample.Tests.Fixtures
{
    class RestaurantsFixtures
    {
        [FixtureLoader]
        public static void Restaurants(dynamic fixtures, DbContext context)
        {
            fixtures.mishkinsRestaurant = new Restaurant()
            {
                Id = 1,
                Name = "Mishkin's Amazing Italian Food",
                OpeningTime = DateTime.Now.AddHours(-2),
                ClosingTime = DateTime.Now.AddHours(2)
            };

            context.Set<Restaurant>().Add(fixtures.mishkinsRestaurant);
            context.SaveChanges();

            // Add to our test context
            TestRepositories.AddFixtureToContext(fixtures.mishkinsRestaurant);
        }
    }
}
