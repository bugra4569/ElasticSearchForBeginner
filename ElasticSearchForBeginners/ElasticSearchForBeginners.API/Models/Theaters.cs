using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchForBeginners.API.Models
{
    public class Theaters
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }             
        public string Summary { get; set; }
    }
}
