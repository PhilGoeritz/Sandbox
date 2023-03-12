using Sandbox.DatabaseApp;
using Sandbox.DatabaseApp.Model;

Console.WriteLine("First clear DB");
var firstDbContext = new OrderContext();
firstDbContext.RemoveRange(firstDbContext.Products);
firstDbContext.RemoveRange(firstDbContext.Orders);
firstDbContext.SaveChanges();
firstDbContext.Dispose();

Console.WriteLine("Then create new order with products");
var product = new Product(1, "A fun toy");
var order = new Order(2, new List<Product> { product });
var secondDbContext = new OrderContext();
secondDbContext.Add(order);
secondDbContext.SaveChanges();
secondDbContext.Dispose();

var thrirdDbContext = new OrderContext();
var productFromDb = thrirdDbContext.Products.Find(1);
Console.WriteLine($"Then {productFromDb?.Name ?? "ERROR - NOTHING FOUND"} can be found in DB");