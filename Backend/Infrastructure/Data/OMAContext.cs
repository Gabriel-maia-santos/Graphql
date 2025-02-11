using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class OMAContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public OMAContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "John Doe",
                    LastName = "Doe",
                    ContactNumber = "1234567890",
                    Email = "jgjgj@jgjgj",
                    IsDeleted = false
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Jane Doe",
                    LastName = "Doe",
                    ContactNumber = "0987654321",
                    Email = "jane.doe@jane.com",
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    CustomerId = 1,
                    AddressLine = "123 Main St",
                    AddressLine2 = "Apt 4B",
                    City = "Anytown",
                    State = "CA",
                    Country = "USA"
                },
                new Address
                {
                    Id = 2,
                    CustomerId = 2,
                    AddressLine = "456 Elm St",
                    AddressLine2 = "Suite 101",
                    City = "Othertown",
                    State = "NY",
                    Country = "USA"
                }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2022, 10, 20),
                    Description = "Order 1",
                    TotalAmount = 500,
                    DepositAmount = 100,
                    IsDelivery = true,
                    Status = Status.Pending,
                    OtherNotes = "Order 1 notes",
                    IsDeleted = false
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 2,
                    OrderDate = new DateTime(2022, 11, 12),
                    Description = "Order 2",
                    TotalAmount = 1000,
                    DepositAmount = 200,
                    IsDelivery = false,
                    Status = Status.Draft,
                    OtherNotes = "Order 2 notes",
                    IsDeleted = false
                }
            );
        }
    }
}