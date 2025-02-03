﻿using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Couple> Couples { get; set; }
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

