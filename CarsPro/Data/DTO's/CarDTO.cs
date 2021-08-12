using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Data.DTO_s
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int? FactoryId { get; set; }
        public virtual FactoryDTO Factory { get; set; }
        
    }
}
