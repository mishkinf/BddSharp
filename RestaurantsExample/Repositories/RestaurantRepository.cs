using BddSharp.Web;
using RestaurantsExample.EntityFramework;
using RestaurantsExample.Models;
using System.Data.Entity;

namespace RestaurantsExample.Repositories
{
    public class RestaurantRepository : GenericRepository<Restaurant>
    {
        public RestaurantRepository() : base(new RestaurantsContext())
        {
                
        }

        public RestaurantRepository(DbContext context) : base(context)
        {
        }      
    }
}