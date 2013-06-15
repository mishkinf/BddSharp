using BddSharp.Web;
using RestaurantsExample.EntityFramework;
using RestaurantsExample.Models;
using RestaurantsExample.Repositories;
using System;
using System.Linq;

namespace RestaurantsExample.Managers
{
    public class RestaurantManager
    {
        public IRepository<Restaurant> restaurantRepository;

        public RestaurantManager()
        {
            restaurantRepository = new RestaurantRepository();
        }

        public Restaurant[] GetOpenRestaurants(DateTime time)
        {
            return restaurantRepository.Entities.Where(x => x.OpeningTime <= time && x.ClosingTime > time).ToArray();
        }
    }
}