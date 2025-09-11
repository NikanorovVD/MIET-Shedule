using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataLayer.Configuration
{
    internal class PairConfiguration : IEntityTypeConfiguration<Pair>
    {
        public void Configure(EntityTypeBuilder<Pair> builder)
        {
            builder.HasOne<Teacher>(p => p.Teacher)
                .WithMany()
                .HasForeignKey(p => p.TeacherId);

            builder.HasOne<Group>(p => p.Group)
                .WithMany()
                .HasForeignKey(p => p.GroupId);

            builder.HasOne<TimePair>(p => p.Time)
                .WithMany()
                .HasForeignKey(p => p.Order);
        }
    }
}
