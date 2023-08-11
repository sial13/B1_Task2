using B1_Task2.Models;
using MathNet.Numerics.RootFinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace B1_Task2.Models.Data
{
    public class AppDbContext : DbContext
    {

        private string _currentTableName;
        public DbSet<Balances> Balances { get; set; }

        public AppDbContext(string tableName)
        {
            _currentTableName = tableName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Balance;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if (!string.IsNullOrEmpty(_currentTableName))
            {
                modelBuilder.Entity<Balances>().ToTable(_currentTableName);
            }
            modelBuilder.Entity<Balances>().HasNoKey();
        }

    }
}
