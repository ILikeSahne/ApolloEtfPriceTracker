using ApolloEtfPricesWebApi.Database;
using ApolloEtfPricesWebApi.Database.Price;
using FinanzenAtWebScraper.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseAccess>(options => options.UseMongoDB(new MongoClient("mongodb://localhost:27017"), "pension_investment_tracker"));

builder.Services.AddTransient<PriceTable>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("api/v1/prices", async ([FromServices] PriceTable priceTable, CancellationToken ct) =>
{
    return Results.Ok(await priceTable.GetAllPricesAsync(ct));
});

app.MapPut("api/v1/addPrice", async ([FromBody] PriceObject price, [FromServices] PriceTable priceTable, CancellationToken ct) =>
{
    var result = await priceTable.AddPriceAvoidDuplicatesAsync(price, ct);

    Console.WriteLine($"[{price.DateAndTime}] Adding Price: {price.Price}");

    if (result)
    {
        return Results.Ok();
    }

    return Results.Conflict("A price for the specified time already exists in the Database");
});

app.Run();