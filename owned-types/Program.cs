﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            RecreateDatabase();

            Console.WriteLine("Inserting Data......");

            using (var db = new CustomerContext())
            {
                db.Customers.Add(
                    new Customer
                    {
                        Name = "Diego",
                        WorkAddress = new Address
                        {
                            LineOne = "Microsoft Campus",
                            LineTwo = "One Microsoft Way",
                            CityOrTown = "Redmond",
                            PostalOrZipCode = "98052",
                            StateOrProvince = "WA",
                            CountryName = "United States of America"
                        },
                        HomeAddress = new Address
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

            Console.WriteLine("done");
            Console.Read();
        }

        private static void RecreateDatabase()
        {
            using (var db = new CustomerContext())
            {
                Console.WriteLine("Recreating database from current model");
                Console.WriteLine(" Dropping database...");

                db.Database.EnsureDeleted();

                Console.WriteLine("done");

                Console.WriteLine(" Creating database...");

                db.Database.EnsureCreated();

                Console.WriteLine("done");
            }
        }
    }

    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Demo.OwnedTypes;Trusted_Connection=True;ConnectRetryCount=0;")
                .UseLoggerFactory(new LoggerFactory().AddConsole((s, l) => l == LogLevel.Information && !s.EndsWith("Connection")));
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
        public Address HomeAddress { get; set; }
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
