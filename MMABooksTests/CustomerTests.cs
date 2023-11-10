using NUnit.Framework;
//using MMABooksEFClasses.MarisModels;
using MMABooksEFClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests {
    [TestFixture]
    public class CustomerTests {
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup() {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest() {
            customers = dbContext.Customers.OrderBy(c => c.Name).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetByPrimaryKeyTest() {
            c = dbContext.Customers.Find(121);
            Assert.IsNotNull(c);
            Assert.AreEqual("Demith, Bob", c?.Name);
            Console.WriteLine(c);
        }

        [Test]
        public void GetUsingWhere() {
            customers = dbContext.Customers.Where(c => c.State.Equals("OR")).OrderBy(c => c.Name).ToList();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("Erpenbach, Lee", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetWithInvoicesTest() {
            c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();
            Assert.IsNotNull(c);
            Assert.AreEqual("Chamberland, Sarah", c?.Name);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void GetWithJoinTest() {
            var customers = dbContext.Customers.Join(dbContext.States, c => c.State, s => s.StateCode, (c, s) => new { c.CustomerId, c.Name, c.State, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(696, customers.Count);
            foreach (var c in customers) Console.WriteLine(c);
        }

        [Test]
        public void DeleteTest() {
            c = dbContext.Customers.Find(1);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(1));
        }

        [Test]
        public void CreateTest() {
            c = new Customer {
                CustomerId = 1000,
                Name = "1",
                Address = "2",
                City = "3",
                State = "FL",
                ZipCode = "4"
            };
            dbContext.Customers.Add(c);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Customers.Find(1000));
        }

        [Test]
        public void UpdateTest() {
            c = dbContext.Customers.Find(200);
            c.Name = "Name was changed";
            dbContext.Customers.Update(c);
            dbContext.SaveChanges();
            c = dbContext.Customers.Find(200);
            Assert.AreEqual("Name was changed", c?.Name);
        }

        public static void PrintAll(List<Customer> customers) { foreach (Customer c in customers) Console.WriteLine(c); }
    }
}
