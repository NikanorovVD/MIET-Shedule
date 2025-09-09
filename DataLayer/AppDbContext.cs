using DataLayer.Configuration;
using DataLayer.Entities;
using DataLayer.Entities.Virtual;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimePair> TimePairs { get; set; }

        public virtual DbSet<TeacherPair> TeacherPairs { get; set; }

        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PairConfiguration());
            base.OnModelCreating(builder);
        }
    }
}

