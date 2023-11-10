using NUnit.Framework;
//using MMABooksEFClasses.MarisModels;
using MMABooksEFClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests {
    [TestFixture]
    public class ProductTests {
        MMABooksContext dbContext;
        Product? p;
        List<Product>? products;

        [SetUp]
        public void Setup() {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest() {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest() {
            p = dbContext.Products.Find("CS10");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's C# 2010", p?.Description);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere() {
            products = dbContext.Products.Where(p => p.UnitPrice.Equals(56.50m)).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual(4637, products[0].OnHandQuantity);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest() {
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products) Console.WriteLine(p);
        }

        [Test]
        public void DeleteTest() {
            // Reason why I'm creating a new product instead of referencing an existing product: https://classes.lanecc.edu/mod/forum/discuss.php?d=951229#p2624389
            p = new Product {
                ProductCode = "ABC2",
                Description = "Book 1",
                UnitPrice = 10.0000m,
                OnHandQuantity = 1234
            };
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("ABC2"));
        }

        [Test]
        public void CreateTest() {
            p = new Product {
                ProductCode = "ABC1",
                Description = "Book 1",
                UnitPrice = 10.0000m,
                OnHandQuantity = 1234
            };
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            Console.WriteLine(p);
            Assert.IsNotNull(dbContext.Products.Find("ABC1"));
        }

        [Test]
        public void UpdateTest() {
            p = dbContext.Products.Find("DB2R");
            p.Description = "Description was changed";
            dbContext.Products.Update(p);
            dbContext.SaveChanges();
            p = dbContext.Products.Find("DB2R");
            Assert.AreEqual("Description was changed", p?.Description);
        }

        public static void PrintAll(List<Product> products) { foreach (Product p in products) Console.WriteLine(p); }
    }
}
