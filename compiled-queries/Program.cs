using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Performance.EFCore;

namespace Demos
{
    internal class Program
    {
        private static void Main()
        {
            // Warmup
            using (var db = new AdventureWorksContext())
            {
                var customer = db.Customers.First();
            }

            RunTest(
                accountNumbers =>
                {
                    using (var db = new AdventureWorksContext())
                    {
                        foreach (var id in accountNumbers)
                        {
                            var customer = db.Customers.Single(c => c.AccountNumber == id);
                        }
                    }
                },
                accountNumbers =>
                {
                    var query = EF.CompileQuery((AdventureWorksContext db, string id) =>
                        db.Customers.Single(c => c.AccountNumber == id));

                    using (var db = new AdventureWorksContext())
                    {
                        foreach (var id in accountNumbers)
                        {
                            var customer = query(db, id);
                        }
                    }
                });
        }

        private static void RunTest(Action<string[]> regularTest, Action<string[]> compiledTest)
        {
            var accountNumbers = GetAccountNumbers(500);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            regularTest(accountNumbers);
            stopwatch.Stop();
            var regularResult = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Regular:  {regularResult.ToString().PadLeft(4)}ms");

            stopwatch.Restart();
            compiledTest(accountNumbers);
            stopwatch.Stop();
            var compiledResult = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Compiled: {compiledResult.ToString().PadLeft(4)}ms");
        }

        private static string[] GetAccountNumbers(int count)
        {
            var accountNumbers = new string[count];

            for (var i = 0; i < count; i++)
            {
                accountNumbers[i] = "AW" + (i + 1).ToString().PadLeft(8, '0');
            }

            return accountNumbers;
        }
    }
}