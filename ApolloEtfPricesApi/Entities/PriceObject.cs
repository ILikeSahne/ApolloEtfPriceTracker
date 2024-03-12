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

    public PriceObject(double price) : this(DateTime.Now, price)
    { }

    public PriceObject() : this(0)
    { }

    public PriceObject FromCsvLine(string line)
    {
        var args = line.Split(';');

        var date = DateTime.ParseExact(args[0], "dd/MM/yyyy", null);
        var time = TimeSpan.Parse(args[1]);

        var dateAndTime = date.Add(time);

        return new PriceObject(dateAndTime, double.Parse(args[2]));
    }
}
