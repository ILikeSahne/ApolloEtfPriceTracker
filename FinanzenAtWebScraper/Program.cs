using HtmlAgilityPack;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System.Globalization;
using System.Net;
using FinanzenAtWebScraper.Database;

#if DEBUG
args = new[] { "C:\\Users\\jonat\\Desktop\\Projects\\GRAWE Pensions Vorsorge App\\PensionInvestmentTracker\\FinanzenAtWebScraper\\past\\past_performance_only_price_until_08_03_2024.csv" };
#endif

var client = new MongoClient("mongodb://localhost:27017");
var db = PensionInvestmentTrackerDbContext.Create(client.GetDatabase("pension_investment_tracker"));

var priceTable = new PriceTable(db);

if (args.Length >= 1)
{
    var importCsv = args[0];

    var data = await File.ReadAllLinesAsync(importCsv);

    var parsedData = data.Skip(1).Select(PriceSet.FromCsvLine).ToArray();

    await priceTable.AddPricesAvoidDuplicatesAsync(parsedData);
}

using (HttpClient httpClient = new())
{
    while(true)
    {
        string html = await httpClient.GetStringAsync("https://markets.ft.com/data/funds/tearsheet/summary?s=AT0000708771:EUR");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var value = double.Parse(htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/section[1]/div/div/div[1]/div[2]/ul/li[1]/span[2]").InnerText, new CultureInfo("en"));

        await priceTable.AddPriceAvoidDuplicatesAsync(PriceSet.FromPrice(value));

        Console.WriteLine($"{DateTime.Now}: {value}€");

        await Task.Delay(1000 * 60 * 15);
    }
}