namespace Stonks
{
    public class StockModel
    {
        public string Name { get; }
        public string Symbol { get; }
        public string Value { get; }
        public string Change { get; }

        public StockModel(string name, string symbol, string value, string change)
        {
            Name = name;
            Symbol = symbol;
            Value = value;
            Change = change;
        }
    }
}