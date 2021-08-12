using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Entity
{
    public class Factory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public virtual IList<Car> Cars { get; set; } = new List<Car>();
    }
}
