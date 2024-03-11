using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenAtWebScraper.Database;

internal class PensionInvestmentTrackerDbContext : DbContext
{
    public DbSet<PriceSet> PriceSet { get; set; }

    public static PensionInvestmentTrackerDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<PensionInvestmentTrackerDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    public PensionInvestmentTrackerDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PriceSet>().ToCollection("prices");
    }
}