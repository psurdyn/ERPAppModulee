using ERPAppModuleDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERPAppModuleDb.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<TruckStatusesDictionary>(new TruckStatusesDictionaryConfiguration());
        modelBuilder.ApplyConfiguration<TrucksEntity>(new TrucksEntityConfiguration());
    }
}