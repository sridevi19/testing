using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.CheckBook;
using System.Linq;
using System.Collections.ObjectModel;

namespace CalculatorTests
{
    [TestClass]
    public class CheckBookTest
    {
        [TestMethod]
        public void FillsUpProperly()
        {
            var ob = new CheckBookVM();

            Assert.IsNull(ob.Transactions);

            ob.Fill();

            Assert.AreEqual(12, ob.Transactions.Count);
        }

        [TestMethod]
        public void CountofEqualsMoshe()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var count = ob.Transactions.Where( t => t.Payee == "Moshe" ).Count();

            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void SumOfMoneySpentOnFood()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var category = "Food";

            var food = ob.Transactions.Where(t=> t.Tag == category );

            var total = food.Sum(t => t.Amount);

            Assert.AreEqual(261, total);

        }

        [TestMethod]
        public void Group()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var total = ob.Transactions.GroupBy(t => t.Tag).Select(g => new { g.Key, Sum=g.Sum( t=> t.Amount ) });

            Assert.AreEqual(261, total.First().Sum);
            Assert.AreEqual(300, total.Last().Sum);
        }

        //-------------------------------- One -------------------------------------------------
        [TestMethod]
        public void AvgTransaction()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var tot = ob.Transactions.GroupBy(s => s.Tag).Select(n => new { n.Key, avg = n.Average(s => s.Amount) });
            Assert.AreEqual(32.625, tot.First().avg);
            Assert.AreEqual(75, tot.Last().avg);
        }


        //-------------------------------- Two -------------------------------------------------
        [TestMethod]
        public void AmountPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var PaymentToMoshe = ob.Transactions.Where(m => m.Payee == "Moshe").Sum(a => a.Amount);
            Assert.AreEqual(130, PaymentToMoshe);

            var PaymentToTim = ob.Transactions.Where(t => t.Payee == "Tim").Sum(a => a.Amount);
            Assert.AreEqual(300, PaymentToTim);

            var PaymentToBracha = ob.Transactions.Where(b => b.Payee == "Bracha").Sum(a => a.Amount);
            Assert.AreEqual(131, PaymentToBracha);
        }


        //-------------------------------- Three -------------------------------------------------
        [TestMethod]
        public void SumPayee()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var PaymentToMoshe = ob.Transactions.Where(m => m.Payee == "Moshe" && m.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(130, PaymentToMoshe);

            var PaymentToTim = ob.Transactions.Where(t => t.Payee == "Tim" && t.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(0, PaymentToTim);

            var PaymentToBracha = ob.Transactions.Where(b => b.Payee == "Bracha" && b.Tag == "Food").Sum(a => a.Amount);
            Assert.AreEqual(131, PaymentToBracha);
        }


        //-------------------------------- Four -------------------------------------------------
        [TestMethod]
        public void TransactionDates()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var TransactionCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8")).Count();
            Assert.AreEqual(6, TransactionCount);
        }


        //-------------------------------- Five -------------------------------------------------
        [TestMethod]
        public void AccountUsed()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var AccountDates = ob.Transactions.Select(d => new { d.Date, d.Account }).Count();
            Assert.AreEqual(12, AccountDates);
        }


        //-------------------------------- Six -------------------------------------------------
        [TestMethod]
        public void MoneyOnAuto()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var Checking = ob.Transactions.Where(c => c.Tag == "Auto" && c.Account == "Checking").Sum(a => a.Amount);
            var Credittot = ob.Transactions.Where(h => h.Tag == "Auto" && h.Account == "Credit").Sum(a => a.Amount);
            Assert.IsTrue(Checking == Credittot);
        }


        //-------------------------------- Seven -------------------------------------------------
        [TestMethod]
        public void TransacsBwnAccount()
        {
            var ob = new CheckBookVM();
            ob.Fill();

            var CheckingsCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8") && d.Account == "Checking").Count();
            Assert.AreEqual(3, CheckingsCount);

            var CreditCount = ob.Transactions.Where(d => d.Date >= DateTime.Parse("2015-4-5") && d.Date < DateTime.Parse("2015-4-8") && d.Account == "Credit").Count();
            Assert.AreEqual(3, CreditCount);
        }
    }
}
