using BddSharp;
using RestaurantsExample.Models;
using System;
using System.Data.Entity;

namespace RestaurantsExample.Tests.Fixtures
{
    class RestaurantsFixtures : DataFixture
    {
        [FixtureLoader]
        public static void Restaurants()
        {
            Add("mishkinsRestaurant", 
            new Restaurant()
            {
                Id = 1,
                Name = "Mishkin's Amazing Italian Food",
                OpeningTime = DateTime.Now.AddHours(-2),
                ClosingTime = DateTime.Now.AddHours(2)
            });
        }
    }
}
