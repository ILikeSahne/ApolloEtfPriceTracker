using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenAtWebScraper.Database;

public record PriceSet(ObjectId Id, DateTime DateAndTime, double Price)
{
    public static PriceSet FromPrice(double price)
    {
        return new PriceSet(ObjectId.GenerateNewId(), DateTime.Now, price);
    }

    public static PriceSet FromCsvLine(string line)
    {
        var args = line.Split(';');

        var date = DateTime.ParseExact(args[0], "dd/MM/yyyy", null);
        var time = TimeSpan.Parse(args[1]);

        var dateAndTime = date.Add(time);

        return new PriceSet(ObjectId.GenerateNewId(), dateAndTime, double.Parse(args[2]));
    }
}