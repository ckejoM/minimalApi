using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new List<Product>()
{
    new Product() { Id = 1, ProductName = "Smart Phone"},
    new Product() { Id = 2, ProductName = "Smart TV"},
};

app.MapGet("/products", async (HttpContext context) => {
    var cities = string.Join("\n", products.Select(x => x.ToString()));

    await context.Response.WriteAsync(cities);
});

app.MapPost("/products", async (HttpContext context, Product product) =>
{
    products.Add(product);
    await context.Response.WriteAsync("Product added");
});

app.Run();
