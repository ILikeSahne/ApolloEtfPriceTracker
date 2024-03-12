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

    /// <summary>
    /// Adds a Price to the Database
    /// </summary>
    /// <param name="price">Time and Price of the ETF</param>
    /// <param name="ct">CancellationToken to cancel request if needed</param>
    /// <returns>True when Price was added to Database</returns>
    public async Task<bool> AddPriceAvoidDuplicatesAsync(PriceObject price, CancellationToken ct = default)
    {
        if (Database.PriceSet.Any(p => p.DateAndTime == price.DateAndTime))
        {
            return false;
        }

        await Database.PriceSet.AddAsync(new PriceSet(price), ct);
        await Database.SaveChangesAsync(ct);

        return true;
    }

    public async Task<List<PriceObject>> GetAllPricesAsync(CancellationToken ct = default)
    {
        return (await Database.PriceSet.ToListAsync(ct)).Select(p => p.ToPriceObject()).OrderByDescending(p => p.DateAndTime).ToList();
    }
}