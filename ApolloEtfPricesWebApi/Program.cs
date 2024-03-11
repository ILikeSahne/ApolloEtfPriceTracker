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

app.MapGet("/prices", async ([FromServices] PriceTable priceTable, CancellationToken ct) =>
{
    return Results.Ok(await priceTable.GetAllPricesAsync(ct));
});

app.MapPut("/addPrice", async ([FromBody] PriceObject price, [FromServices] PriceTable priceTable, CancellationToken ct) =>
{
    await priceTable.AddPriceAvoidDuplicatesAsync(price, ct);

    return Results.Ok();
});

app.Run();