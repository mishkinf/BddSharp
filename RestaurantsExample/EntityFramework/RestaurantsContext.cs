using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RestaurantsExample.Models;

namespace RestaurantsExample.EntityFramework
{
    public class RestaurantsContext : DbContext
    {
        private static RestaurantsContext instance;

        private RestaurantsContext() : base("name=DefaultConnection") { }

        private RestaurantsContext(string connectionString) : base(connectionString) { }

        public DbSet<Restaurant> Restaurants { get; set; }

        public static RestaurantsContext Instance
        {
            get
            {
                if (instance == null)
                {
                    string testConn = "Data Source=.;Initial Catalog=RestaurantTestExample;Integrated Security=True";

                    instance = new RestaurantsContext(testConn);
                }

                return instance;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                        .HasKey(x => x.Id);

            modelBuilder.Entity<Restaurant>()
                .Property(x => x.Name)
                .HasColumnType("varchar");

            modelBuilder.Entity<Restaurant>()
                .Property(x => x.OpeningTime)
                .HasColumnType("datetime");

            modelBuilder.Entity<Restaurant>()
                .Property(x => x.ClosingTime)
                .HasColumnType("datetime");

            modelBuilder.Entity<Restaurant>()
                .ToTable("Restaurants");
        }

    }
}