using MinimalAPI.Models;
using MinimalAPI.RouteGroups;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//var products = new List<Product>()
//{
//    new Product() { Id = 1, ProductName = "Smart Phone"},
//    new Product() { Id = 2, ProductName = "Smart TV"},
//};

//app.MapGet("/products", async (HttpContext context) => {
//    var productsResponse = string.Join("\n", products.Select(x => x.ToString()));

//    await context.Response.WriteAsync(productsResponse);
//});

////GET /products/{id}
//app.MapGet("/products/{id:int}", async (HttpContext context, int id) => {
//    var product = products.FirstOrDefault(x => x.Id == id);

//    if (product is null)
//    {
//        context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Incorrect Product Id");
//        return;
//    }

//    await context.Response.WriteAsync(JsonSerializer.Serialize(product.ToString()));
//});

//app.MapPost("/products", async (HttpContext context, Product product) =>
//{
//    products.Add(product);
//    await context.Response.WriteAsync("Product added"); 
//});

var mapGroup = app.MapGroup("/productsGroup").ProductsAPI();

app.Run();
