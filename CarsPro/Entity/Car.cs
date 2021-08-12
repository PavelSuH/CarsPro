using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Entity
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int? FactoryId { get; set; }
        public virtual Factory Factory { get; set; }

        
    }
}
