using ERPAppModuleDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERPAppModule.Data;

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