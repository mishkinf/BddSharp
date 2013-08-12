using System;
using System.Data.Entity;
using BddSharp.Web;

namespace RestaurantsExample.EntityFramework
{
    public class RestaurantsSeedInitializer : IDatabaseInitializer<RestaurantsContext>
    {
        private Action<RestaurantsContext> FixtureSeeder;

        public RestaurantsSeedInitializer()
        {
        }

        public RestaurantsSeedInitializer(Action<RestaurantsContext> fixtureSeeder)
        {
            FixtureSeeder = fixtureSeeder;
        }

        protected void Seed(RestaurantsContext context)
        {
            // Clear the DB and load fixtures
            if (FixtureSeeder != null)
            {
//                BddHelpers.ClearDatabase(context);
                FixtureSeeder(context);
            }
        }

        public void InitializeDatabase(RestaurantsContext context)
        {
            Seed(context);
        }
    }
}