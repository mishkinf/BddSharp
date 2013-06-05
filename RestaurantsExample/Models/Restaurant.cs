using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantsExample.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }
    }
}