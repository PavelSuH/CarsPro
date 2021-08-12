using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Data.DTO_s
{
    public class FactoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public virtual IList<CarDTO> CarsDTO { get; set; } = new List<CarDTO>();
    }
}
