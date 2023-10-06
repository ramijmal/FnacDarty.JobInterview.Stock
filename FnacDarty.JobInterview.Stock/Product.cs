using System;

namespace FnacDarty.JobInterview.Stock
{
    public class Product
    {
        public string EAN { get; }
        public int Stock { get; set; }

        public Product(string ean)
        {
            if (ean.Length != 8)
            {
                throw new ArgumentException("EAN must be 8 characters long.");
            }
            EAN = ean;
            Stock = 0;
        }
    }
}
