using BddSharp.Web;
using RestaurantsExample.Models;
using System.Data.Entity;

namespace RestaurantsExample.Repositories
{
    public class RestaurantRepository : GenericRepository<Restaurant>
    {
        public RestaurantRepository(DbContext context) : base(context)
        {
        }      
    }
}