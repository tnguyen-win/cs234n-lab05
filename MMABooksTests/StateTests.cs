using NUnit.Framework;
//using MMABooksEFClasses.MarisModels;
using MMABooksEFClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests {
    [TestFixture]
    public class StateTests {
        MMABooksContext dbContext;
        State? s;
        List<State>? states;

        [SetUp]
        public void Setup() {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest() {
            states = dbContext.States.OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(53, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetByPrimaryKeyTest() {
            s = dbContext.States.Find("OR");
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s?.StateName);
            Console.WriteLine(s);
        }

        [Test]
        public void GetUsingWhere() {
            states = dbContext.States.Where(s => s.StateName.StartsWith("A")).OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(4, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetWithCustomersTest() {
            s = dbContext.States.Include("Customers").Where(s => s.StateCode == "OR").SingleOrDefault();
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s?.StateName);
            Assert.AreEqual(5, s.Customers.Count);
            Console.WriteLine(s);
        }

        [Test]
        public void DeleteTest() {
            s = dbContext.States.Find("ZZ");
            dbContext.States.Remove(s);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.States.Find("ZZ"));
        }

        [Test]
        public void CreateTest() {
            s = new State {
                StateCode = "ZZ",
                StateName = "ZooZoo"
            };
            dbContext.States.Add(s);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.States.Find("ZZ"));
        }

        [Test]
        public void UpdateTest() {
            s = dbContext.States.Find("OR");
            s.StateName = "Ore";
            dbContext.States.Update(s);
            dbContext.SaveChanges();
            s = dbContext.States.Find("OR");
            Assert.AreEqual("Ore", s?.StateName);
        }

        public static void PrintAll(List<State> states) { foreach (State s in states) Console.WriteLine(s); }
    }
}
