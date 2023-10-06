using System.Collections.Generic;
using System;
using System.Linq;

namespace FnacDarty.JobInterview.Stock
{
    public class Stock
    {
        public List<Product> MyProducts { get; private set; }
        public List<StockMovement> Movements { get; private set; }

        public Stock()
        {
            this.Movements = new List<StockMovement>();
            this.MyProducts = new List<Product>();
        }

        public void AddProduct(string ean)
        {
            if (!this.MyProducts.Any(x => x.EAN == ean))
            {
                this.MyProducts.Add(new Product(ean));
            }

        }

        public void RecordStockMovement(StockMovement movement)
        {
            if (this.Movements.Any(x => movement.Date <= x.Date))
            {
                throw new InvalidOperationException("Cannot add movement on or before an inventory date.");
            }

            foreach (var productmovement in movement.ProductMovements)
            {
                Product product = productmovement.Key;
                int quantity = productmovement.Value;

                var currentProduct = this.MyProducts.FirstOrDefault(x => x.EAN == product.EAN);

                if (currentProduct == null)
                {
                    throw new InvalidOperationException("Product does not exist.");
                }
                else
                {
                    if (movement.Type == 'I') // Inventaory
                    {
                        currentProduct.Stock = quantity;
                    }
                    else
                    {
                        currentProduct.Stock += quantity;
                    }
                }

            }
            this.Movements.Add(movement);
        }

        public Dictionary<string, int> GetStockAtDate(DateTime date)
        {
            var stockAtDate = new Dictionary<string, int>();

            foreach (var product in this.MyProducts)
            {
                int stock = 0;
                foreach (var movement in this.Movements.Where(m => m.Date <= date))
                {
                    if(movement.ProductMovements.ContainsKey(product))
                    {
                        int quantity = movement.ProductMovements[product];
                        stock += quantity;
                    }
                }

                if (stock < 0)
                    stock = 0;

                stockAtDate[product.EAN] = stock;
            }

            return stockAtDate;
        }

        public Dictionary<string, int> GetStockVariations(DateTime startDate, DateTime endDate)
        {
            var stockVariations = new Dictionary<string, int>();

            foreach (var product in this.MyProducts)
            {
                int variation = 0;

                foreach (var movement in this.Movements)
                {
                    if (movement.Date >= startDate && movement.Date <= endDate)
                    {
                        if (movement.ProductMovements.ContainsKey(product))
                        {
                            int quantity = movement.ProductMovements[product];
                            variation += quantity;
                        }
                    }
                }

                stockVariations[product.EAN] = variation;
            }

            return stockVariations;
        }

        public Dictionary<string, int> GetCurrentStock()
        {
            var currentStock = new Dictionary<string, int>();

            foreach (var product in this.MyProducts)
            {
                int stock = product.Stock < 0 ? 0 : product.Stock;
                currentStock[product.EAN] = stock;
            }

            return currentStock;
        }

        public List<string> GetProductsInStock()
        {
            return this.MyProducts.Where(p => p.Stock >= 0).Select(p => p.EAN).ToList();
        }

        public int GetTotalProductCount()
        {
            return this.MyProducts.Sum(p => Math.Max(p.Stock, 0));
        }
    }
}
