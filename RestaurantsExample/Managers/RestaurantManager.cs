using System;
using System.Linq;
using BddSharp;
using RestaurantsExample.EntityFramework;
using RestaurantsExample.Models;
using RestaurantsExample.Repositories;

namespace RestaurantsExample.Managers
{
    public class RestaurantManager
    {
        public IRepository<Restaurant> restaurantRepository;

        public RestaurantManager()
        {
            restaurantRepository = new RestaurantRepository(RestaurantsContext.Instance);
        }

        public Restaurant[] GetOpenRestaurants(DateTime time)
        {
            return restaurantRepository.Entities.Where(x => x.OpeningTime <= time && x.ClosingTime > time).ToArray();
        }
    }
}