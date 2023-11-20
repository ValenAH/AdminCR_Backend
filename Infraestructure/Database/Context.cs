using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database
{
    public class Context : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<IdentificationType> IdentificationType { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Quota> Quota { get; set; }
        public Context(DbContextOptions<Context> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
