using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.JobInterview.Stock
{
    public class StockMovement
    {
        public DateTime Date { get; }
        public string Label { get; }
        public char Type { get; }
        public Dictionary<Product, int> ProductMovements { get; }

        public StockMovement(DateTime date, string label, char type)
        {
            Date = date;
            Label = label;
            ProductMovements = new Dictionary<Product, int>();
            Type = type; // "I" : Inventory ; "C" Movement Classic 
        }

        public void AddProductMovement(Product product, int quantity)
        {
            ProductMovements[product] = quantity;
        }
    }
}
