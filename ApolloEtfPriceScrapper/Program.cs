using HtmlAgilityPack;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using System.Globalization;
using System.Net;
using ApolloEtfPricesWebApi.Database.Price;
using ApolloEtfPricesApiAccessLibrary;

#if DEBUG
args = new[] { "C:\\Users\\jonat\\Desktop\\Projects\\GRAWE Pensions Vorsorge App\\PensionInvestmentTracker\\ApolloEtfPriceScrapper\\past\\past_performance_only_price_until_08_03_2024.csv" };
#endif

var api = new ApolloEtfPricesApi("http://localhost:5205");

if (args.Length >= 1)
{
    var importCsv = args[0];

    var data = await File.ReadAllLinesAsync(importCsv);

    var parsedData = data.Skip(1).Select(d => new PriceObject()).ToArray();

    // Add to database
}

using (HttpClient httpClient = new())
{
    while(true)
    {
        string html = await httpClient.GetStringAsync("https://markets.ft.com/data/funds/tearsheet/summary?s=AT0000708771:EUR");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var price = double.Parse(htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/section[1]/div/div/div[1]/div[2]/ul/li[1]/span[2]").InnerText, new CultureInfo("en"));

        var priceObject = new PriceObject(price);

        try
        {
            var response = await api.AddPriceAsync(priceObject);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Scraping Data failed, trying again in 5 Minutes");
                await Task.Delay(1000 * 60 * 5);

                continue;
            }

            Console.WriteLine($"[{priceObject.DateAndTime}] Scraped Data: {priceObject.Price}€");

            await Task.Delay(1000 * 60 * 15);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Scraping Data failed, maybe Web Api is offline? Trying again in 5 Minutes");
            await Task.Delay(1000 * 60 * 5);
        }
    }
}