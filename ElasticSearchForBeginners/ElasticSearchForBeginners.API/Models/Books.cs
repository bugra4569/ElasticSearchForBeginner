using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchForBeginners.API.Models
{
    public class Books
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public decimal PublishYear { get; set; }
        public bool IsHardCase { get; set; }
        public decimal Price { get; set; }
        public string Summary { get; set; }
    }
}
