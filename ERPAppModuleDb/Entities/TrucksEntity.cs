using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPAppModuleDb.Entities;

public class TrucksEntity : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }

    public int StatusId { get; set; }
    public virtual TruckStatusesDictionary Status { get; set; }
}

internal class TrucksEntityConfiguration : IEntityTypeConfiguration<TrucksEntity>
{
    public void Configure(EntityTypeBuilder<TrucksEntity> builder)
    {
        builder.ToTable("Trucks");
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Code).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()").IsRequired();

        builder.HasOne<TruckStatusesDictionary>(x => x.Status).WithMany(x => x.Trucks).HasForeignKey(x => x.StatusId)
            .HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Restrict);
    }
}