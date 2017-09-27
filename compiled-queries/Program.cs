using System;
using System.Diagnostics;
using System.Linq;
using Performance.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Demos
{
    public class Program
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
                                // Use a regular auto-compiled query
                                var customer = db.Customers.Single(c => c.AccountNumber == id);
                            }
                        }
                    },
                name: "Regular");

            RunTest(
                accountNumbers =>
                    {
                        // Create explicit compiled query


                        using (var db = new AdventureWorksContext())
                        {
                            foreach (var id in accountNumbers)
                            {
                                // Invoke the compiled query

                            }
                        }
                    },
                name: "Compiled");
        }

        private static void RunTest(Action<string[]> test, string name)
        {
            var accountNumbers = GetAccountNumbers(500);
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            test(accountNumbers);

            stopwatch.Stop();

            Console.WriteLine($"{name}:  {stopwatch.ElapsedMilliseconds.ToString().PadLeft(4)}ms");
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
