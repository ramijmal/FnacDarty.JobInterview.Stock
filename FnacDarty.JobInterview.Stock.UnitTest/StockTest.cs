using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FnacDarty.JobInterview.Stock.UnitTest
{
    [TestClass]
    public class StockTest
    {
        [TestMethod]
        public void AddProduct_NewProduct_ProductAdded()
        {
            // Arrange
            var stock = new Stock();
            string ean = "EAN00123";

            stock.AddProduct(ean);

            // Act & Assert
            Assert.IsTrue(stock.MyProducts.Any(x => x.EAN == ean));
        }

        [TestMethod]
        public void RecordStockMovement_ValidMovement_StockUpdated()
        {
            // Arrange
            var stock = new Stock();
            var product = new Product("EAN00456");
            stock.MyProducts.Add(product);
            var movement = new StockMovement(DateTime.Now, "TestMovement", 'C');
            movement.AddProductMovement(new Product("EAN00456"), 10);
            stock.RecordStockMovement(movement);

            // Act & Assert
            Assert.AreEqual(10, product.Stock);
;
        }
        [TestMethod]
        public void RecordStockMovement_ValidMovement_StockDeleted()
        {
            // Arrange
            var stock = new Stock();
            var product = new Product("EAN00456");
            stock.MyProducts.Add(product);
            var movement = new StockMovement(DateTime.Now, "TestMovement", 'C');
            movement.AddProductMovement(new Product("EAN00456"), 10);
            movement.AddProductMovement(new Product("EAN00456"), -5);
            stock.RecordStockMovement(movement);


            // Act & Assert
            Assert.AreEqual(5, product.Stock);
        }




        [TestMethod]
        public void RecordStockMovement_AllStockatdate()
        {
            // Arrange
            var stock = new Stock();
            var product = new Product("EAN01456");
            stock.MyProducts.Add(product);
            var movement = new StockMovement(DateTime.Today, "Test1", 'C');
            movement.AddProductMovement(product, 10);
            stock.RecordStockMovement(movement);
            var movement2 = new StockMovement(DateTime.Today.AddDays(1), "Test2", 'C');
            movement2.AddProductMovement(product, 5);
            stock.RecordStockMovement(movement2);

            var stockAtDate = new Dictionary<string, int>();
            stockAtDate = stock.GetStockAtDate(DateTime.Today);

            // Act & Assert
            Assert.AreEqual(10, stockAtDate[product.EAN]);
        }

        [TestMethod]
        public void RecordStockMovement_AllStockVariation()
        {
            // Arrange
            var stock = new Stock();
            var product = new Product("EAN01456");
            stock.MyProducts.Add(product);
            var movement = new StockMovement(DateTime.Today, "Test1", 'C');
            movement.AddProductMovement(product, 10);
            stock.RecordStockMovement(movement);
            var movement2 = new StockMovement(DateTime.Today.AddDays(1), "Test2", 'C');
            movement2.AddProductMovement(product, 5);
            stock.RecordStockMovement(movement2);

            var stockAtDate = new Dictionary<string, int>();
            stockAtDate = stock.GetStockVariations(DateTime.Today, DateTime.Today.AddDays(1));

            // Act & Assert
            Assert.AreEqual(15, stockAtDate[product.EAN]);
        }


        [TestMethod]
        public void RecordStockMovement_AllStockCurrent()
        {
            // Arrange
            var stock = new Stock();
            var product = new Product("EAN01456");
            stock.MyProducts.Add(product);
            var movement = new StockMovement(DateTime.Today, "Test1", 'C');
            movement.AddProductMovement(product, 10);
            stock.RecordStockMovement(movement);
            var movement2 = new StockMovement(DateTime.Today.AddDays(1), "Test2", 'C');
            movement2.AddProductMovement(product, 5);
            stock.RecordStockMovement(movement2);

            var stockActual = new Dictionary<string, int>();
            stockActual = stock.GetCurrentStock();

            // Act & Assert
            Assert.AreEqual(15, stockActual[product.EAN]);
        }

        [TestMethod]
        public void RecordStockMovement_InvalidDate_ThrowsException()
        {    
            // Arrange
            var stock = new Stock();
            var product = new Product("EAN00789");
            stock.MyProducts.Add(product);

            DateTime inventoryDate = DateTime.Now;
            var inventoryMovement = new StockMovement(inventoryDate, "Inventory", 'I');
            inventoryMovement.AddProductMovement(product, 10);
            stock.RecordStockMovement(inventoryMovement);

            var pastDate = inventoryDate.AddDays(-1);
            var invalidMovement = new StockMovement(pastDate, "InvalidMovement", 'C');
            invalidMovement.AddProductMovement(product, 5);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => stock.RecordStockMovement(invalidMovement));
        }
    }
}
