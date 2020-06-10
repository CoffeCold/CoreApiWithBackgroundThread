using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Models
{


    public class TransactionDBContext : DbContext
    {

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobLog> JobLogs { get; set; }

        public TransactionDBContext(DbContextOptions<TransactionDBContext> options) : base(options)
        {
        }
        public TransactionDBContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO Move it out of source code, only used for populating the test database.
                optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TransactionDB;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>();
            modelBuilder.Entity<JobLog>();
        }
    }
}
