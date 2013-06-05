using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RestaurantsExample.Repositories;

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