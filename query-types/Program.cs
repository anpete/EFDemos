// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            SetupDatabase();

            using (var db = new OrdersContext())
            {
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new OrdersContext())
            {
                db.Database.EnsureDeleted();

                if (db.Database.EnsureCreated())
                {
                    var diego = new Customer { Name = "Diego" };
                    var andrew = new Customer { Name = "Andrew" };

                    db.Set<Customer>().AddRange(diego, andrew);

                    var empanada = new Product { Name = "Empanada" };
                    var meatPie = new Product { Name = "Meat Pie" };

                    db.Set<Product>().AddRange(empanada, meatPie);

                    db.Orders.Add(new Order { Amount = 12, Product = empanada, Customer = diego });
                    db.Orders.Add(new Order { Amount = 20, Product = meatPie, Customer = andrew });

                    db.SaveChanges();

                    db.Database.ExecuteSqlCommand(
                        @"CREATE VIEW [OrderSummary] AS
                            SELECT o.Id, o.Amount, p.Name AS ProductName, c.Name AS CustomerName
                            FROM Orders o
                            INNER JOIN Product p ON o.ProductId = p.Id
                            INNER JOIN Customer c ON o.CustomerId = c.Id");
                }
            }
        }

        public class OrdersContext : DbContext
        {
            private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
                .AddConsole((s, l) => l == LogLevel.Information && s.EndsWith("Command"));

            public DbSet<Order> Orders { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configure a view type
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.ViewTypes;Trusted_Connection=True;ConnectRetryCount=0")
                    .UseLoggerFactory(_loggerFactory);
            }
        }

        public class Order
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public Product Product { get; set; }
            public Customer Customer { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
