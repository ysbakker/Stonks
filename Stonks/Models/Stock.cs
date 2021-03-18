namespace Stonks
{
    public class Stock
    {
        public string Name { get; }
        public string Symbol { get; }

        public Stock(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }
}