using CarsPro.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Data.Config
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.HasKey(c => c.Id);
            builder.Property(c => c.FactoryId).IsRequired();
            
        }
    }
}
