using MongoDB.Bson;

namespace ApolloEtfPricesWebApi.Database.Price;

public class PriceSet
{
    public ObjectId Id { get; set; }

    public DateTime DateAndTime { get; set; }

    public double Price { get; set; }

    public PriceSet(DateTime dateAndTime, double price)
    {
        Id = ObjectId.GenerateNewId();
        DateAndTime = dateAndTime;
        Price = price;
    }

    public PriceSet(PriceObject priceObject) : this(priceObject.DateAndTime, priceObject.Price)
    { }

    public PriceSet(double price) : this(DateTime.Now, price)
    { }

    public PriceSet() : this(0)
    { }

    public PriceObject ToPriceObject()
    {
        return new PriceObject(DateAndTime, Price);
    }
}