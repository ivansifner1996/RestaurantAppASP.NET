using System.Linq;
using RestaurantApp.Models;
using System;

namespace RestaurantApp.Data
{
    public static class DataSeed
    {
        public static void Seed(RestaurantContext context)
        {
            context.Database.EnsureCreated();

            //if (context.Products.Any())
            //{
            //    return;
            //}

            var products = new Product[]
            {
                new Product { Name = "TestProd1", Description = "TestDescription1", Price = 34.25m},
                new Product { Name = "TestProd2", Description = "TestDescription2", Price = 23.22m},
                new Product { Name = "TestProd3", Description = "TestDescription3", Price = 23.22m},
                new Product { Name = "TestProd4", Description = "TestDescription4", Price = 23.22m},
                new Product { Name = "TestProd5", Description = "TestDescription5", Price = 23.22m},
            };

            foreach(Product product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();

            var customers = new Customer[]
            {
                new Customer { CustomerName = "Customer1D.O.O", IBAN="29302930293932", OrderDate=DateTime.Parse("2024-01-01"), OwnerFirstName="Owner1FN", OwnerLastName="Owner1LN"},
                new Customer { CustomerName = "Customer2D.O.O", IBAN="29302930293932", OrderDate=DateTime.Parse("2024-01-01"), OwnerFirstName="Owner2FN", OwnerLastName="Owner2LN"},
                new Customer { CustomerName = "Customer3D.O.O", IBAN="29302930293932", OrderDate=DateTime.Parse("2024-01-01"), OwnerFirstName="Owner3FN", OwnerLastName="Owner3LN"},
                new Customer { CustomerName = "Customer4D.O.O", IBAN="29302930293932", OrderDate=DateTime.Parse("2024-01-01"), OwnerFirstName="Owner4FN", OwnerLastName="Owner4LN"},
                new Customer { CustomerName = "Customer5D.O.O", IBAN="29302930293932", OrderDate=DateTime.Parse("2024-01-01"), OwnerFirstName="Owner5FN", OwnerLastName="Owner5LN"},
            };

            foreach(Customer customer in customers)
            {
                context.Customers.Add(customer);
            }

            context.SaveChanges();

            var orders = new Order[]
            {
                new Order {
                    ProductID = products.Single(s => s.Name == "TestProd1").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer1D.O.O").ID,
                },
                 new Order {
                    ProductID = products.Single(s => s.Name == "TestProd2").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer2D.O.O").ID,
                    Rate = Rating.E
                },
                  new Order {
                    ProductID = products.Single(s => s.Name == "TestProd1").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer3D.O.O").ID,
                },
                   new Order {
                    ProductID = products.Single(s => s.Name == "TestProd2").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer3D.O.O").ID,
                },
                    new Order {
                    ProductID = products.Single(s => s.Name == "TestProd1").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer4D.O.O").ID,
                    Rate= Rating.E
                },
                     new Order {
                    ProductID = products.Single(s => s.Name == "TestProd2").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer4D.O.O").ID,
                },
                      new Order {
                    ProductID = products.Single(s => s.Name == "TestProd3").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer4D.O.O").ID,
                },
                       new Order {
                    ProductID = products.Single(s => s.Name == "TestProd4").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer4D.O.O").ID,
                    Rate = Rating.D
                },
                        new Order {
                    ProductID = products.Single(s => s.Name == "TestProd2").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer5D.O.O").ID,
                    Rate = Rating.A
                },
                         new Order {
                    ProductID = products.Single(s => s.Name == "TestProd4").ProductID,
                    CustomerID = customers.Single(c => c.CustomerName == "Customer5D.O.O").ID,
                }

            };

            foreach (Order order in orders)
            {
                var databaseExistence = context.Orders.Where(
                    o => 
                        o.ProductID == order.ProductID &&
                        o.CustomerID == order.CustomerID
                    ).SingleOrDefault();

                if (databaseExistence == null)
                {
                    context.Orders.Add(order);
                }
            }
            context.SaveChanges();

            var restaurants = new Restaurant[]
            {
                new Restaurant{FoundationDate= DateTime.Parse("2024-01-02"), Country= "UK", RestaurantName="IvanRestaurant1", Address="Test Address 1"},
                new Restaurant{FoundationDate= DateTime.Parse("2024-01-02"), Country= "Croatia", RestaurantName="IvanRestaurant2", Address="Test Address 2"},
                new Restaurant{FoundationDate= DateTime.Parse("2024-01-02"), Country= "USA", RestaurantName="IvanRestaurant3", Address="Test Address 3"},
                new Restaurant{FoundationDate= DateTime.Parse("2024-01-02"), Country= "Germany", RestaurantName="IvanRestaurant4", Address="Test Address 4"},
                new Restaurant{FoundationDate= DateTime.Parse("2024-01-02"), Country= "France", RestaurantName="IvanRestaurant5", Address="Test Address 5"}
            };

            foreach(Restaurant restaurant in restaurants)
            {
                context.Restaurants.Add(restaurant);
            }

            context.SaveChanges();


            var menus = new Menu[]
            {
                new Menu{Name = "Appetizers", Description="My test description 1", RestaurantId= restaurants.Single(restaurant => restaurant.RestaurantName == "IvanRestaurant1").ID },
                new Menu{Name = "Desserts", Description="My test description 2", RestaurantId= restaurants.Single(restaurant => restaurant.RestaurantName == "IvanRestaurant1").ID },
                new Menu{Name = "Vegeterian", Description="My test description 3", RestaurantId= restaurants.Single(restaurant => restaurant.RestaurantName == "IvanRestaurant1").ID },
                new Menu{Name = "Italian", Description="My test description 4", RestaurantId= restaurants.Single(restaurant => restaurant.RestaurantName == "IvanRestaurant1").ID },
                new Menu{Name = "Mexican", Description="My test description 5", RestaurantId= restaurants.Single(restaurant => restaurant.RestaurantName == "IvanRestaurant2").ID },
            };

            foreach(var menu in menus)
            {
                context.Menus.Add(menu);
            }
                

            context.SaveChanges();
        }
    }
}
