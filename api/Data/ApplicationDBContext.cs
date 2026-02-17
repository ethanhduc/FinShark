using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) // constructor that takes in DbContextOptions and passes it to the base DbContext class
        {
            
        }
        public DbSet<Stock> Stocks{ get; set; } // represents the Stocks table in the database
        public DbSet<Comment> Comments{ get; set; }
    }
}