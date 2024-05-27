using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KHCrafts.Models;

namespace KHCrafts.Data
{
    public class KHCraftsContext : DbContext
    {
        public KHCraftsContext (DbContextOptions<KHCraftsContext> options)
            : base(options)
        {
        }

        public DbSet<KHCrafts.Models.Product> Product { get; set; } = default!;
        public DbSet<KHCrafts.Models.Customer> Customer { get; set; } = default!;
        public DbSet<KHCrafts.Models.Order> Order { get; set; } = default!;
    }
}
