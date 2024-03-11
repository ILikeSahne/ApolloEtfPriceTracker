namespace ApolloEtfPricesWebApi.Database.Price;

public class PriceObject
{
    public DateTime DateAndTime { get; set; }
    public double Price { get; set; }

    public PriceObject(DateTime dateAndTime, double price)
    {
        DateAndTime = dateAndTime;
        Price = price;
    }

    public PriceObject(PriceSet priceSet) : this(priceSet.DateAndTime, priceSet.Price)
    { }

    public PriceObject() : this(DateTime.Now, 0)
    { }
}
