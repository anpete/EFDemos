// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            RecreateDatabase();

            Console.Write("Inserting Data......");

            using (var db = new CustomerContext())
            {
                db.Customers.Add(
                    new Customer
                    {
                        Name = "Andrew",
                        WorkAddress = new Address
                        {
                            LineOne = "Microsoft Campus",
                            LineTwo = "One Microsoft Way",
                            CityOrTown = "Redmond",
                            PostalOrZipCode = "98052",
                            StateOrProvince = "WA",
                            CountryName = "United States of America"
                        },
                        PhysicalAddress = new Address
                        {
                            LineOne = "Washington State Convention Center",
                            LineTwo = "705 Pike St",
                            CityOrTown = "Seattle",
                            PostalOrZipCode = "98101",
                            StateOrProvince = "WA",
                            CountryName = "United States of America"
                        }
                    });

                db.SaveChanges();
            }

            Console.WriteLine(" done");
        }

        private static void RecreateDatabase()
        {
            using (var db = new CustomerContext())
            {
                Console.WriteLine("Recreating database from current model");
                Console.Write(" Dropping database...");

                db.Database.EnsureDeleted();

                Console.WriteLine(" done");

                Console.Write(" Creating database...");

                db.Database.EnsureCreated();

                Console.WriteLine(" done");
            }
        }
    }

    public class CustomerContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory()
            .AddConsole((s, l) => l == LogLevel.Information && s.EndsWith("Command"));

        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.OwnedTypes;Trusted_Connection=True;ConnectRetryCount=0;")
                .UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent configuration of owned types.
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public Address WorkAddress { get; set; }
        public Address PhysicalAddress { get; set; }
    }

    public class Address
    {
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string PostalOrZipCode { get; set; }
        public string StateOrProvince { get; set; }
        public string CityOrTown { get; internal set; }
        public string CountryName { get; set; }
    }
}
