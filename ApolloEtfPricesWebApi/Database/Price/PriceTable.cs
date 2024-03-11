using FinanzenAtWebScraper.Database;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ApolloEtfPricesWebApi.Database.Price;

internal class PriceTable(DatabaseAccess Database)
{
    public async Task AddPricesAvoidDuplicatesAsync(IEnumerable<PriceObject> prices, CancellationToken ct = default)
    {
        prices = prices.Where(p => !Database.PriceSet.Any(p2 => p.DateAndTime == p2.DateAndTime));

        await Database.PriceSet.AddRangeAsync(prices.Select(p => new PriceSet(p.DateAndTime, p.Price)), ct);
        await Database.SaveChangesAsync(ct);
    }

    public async Task AddPriceAvoidDuplicatesAsync(PriceObject price, CancellationToken ct = default)
    {
        if (Database.PriceSet.Any(p => p.DateAndTime == price.DateAndTime))
        {
            return;
        }

        await Database.PriceSet.AddAsync(new PriceSet(price), ct);
        await Database.SaveChangesAsync(ct);
    }

    public async Task<List<PriceObject>> GetAllPricesAsync(CancellationToken ct = default)
    {
        return (await Database.PriceSet.ToListAsync(ct)).Select(p => new PriceObject(p)).OrderByDescending(p => p.DateAndTime).ToList();
    }
}