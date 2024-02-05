using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERPAppModuleDb.Entities;

public class TruckStatusesDictionary : BaseEntity
{
    public string Name { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<TrucksEntity> Trucks { get; set; } = new List<TrucksEntity>();
}

internal class TruckStatusesDictionaryConfiguration : IEntityTypeConfiguration<TruckStatusesDictionary>
{
    public void Configure(EntityTypeBuilder<TruckStatusesDictionary> builder)
    {
        builder.ToTable("TruckStatuses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()").IsRequired(false);
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()").IsRequired(false);
    }
}