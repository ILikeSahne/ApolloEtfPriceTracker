using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenAtWebScraper.Database;

internal record PriceTable(PensionInvestmentTrackerDbContext DbContext)
{
    public async Task AddPricesAvoidDuplicatesAsync(IEnumerable<PriceSet> prices)
    {
        prices = prices.Where(p => !DbContext.PriceSet.Any(p2 => p.DateAndTime == p2.DateAndTime));

        await DbContext.PriceSet.AddRangeAsync(prices);
        await DbContext.SaveChangesAsync();
    }

    public async Task AddPriceAvoidDuplicatesAsync(PriceSet price)
    {
        if (DbContext.PriceSet.Any(p => p.DateAndTime == price.DateAndTime))
        {
            return;
        }

        await DbContext.PriceSet.AddAsync(price);
        await DbContext.SaveChangesAsync();
    }
}
