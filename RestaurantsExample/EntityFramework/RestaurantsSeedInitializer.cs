using System;
using System.Data.Entity;

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
            // Populate DB with Fixture data, if passed in
            if (FixtureSeeder != null)
            {
                FixtureSeeder(context);
            }
        }

        public void InitializeDatabase(RestaurantsContext context)
        {
            Seed(context);
        }
    }
}