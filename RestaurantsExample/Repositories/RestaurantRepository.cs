using System.Configuration;
using System.Data.Entity;
using System.Linq;
using BddSharp;
using RestaurantsExample.EntityFramework;
using RestaurantsExample.Models;

namespace RestaurantsExample.Repositories
{
    public class RestaurantRepository : GenericRepository<Restaurant>
    {
        public RestaurantRepository(DbContext context) : base(context)
        {
        }      
    }
}