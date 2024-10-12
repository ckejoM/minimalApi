using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MinimalAPI.RouteGroups
{
    public static class ProductsMapGroup
    {
        private static List<Product> products = new List<Product>()
        {
            new Product() { Id = 1, ProductName = "Smart Phone"},
            new Product() { Id = 2, ProductName = "Smart TV"},
        };

        public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (HttpContext context) => {
                var productsResponse = string.Join("\n", products.Select(x => x.ToString()));

                await context.Response.WriteAsync(productsResponse);
            });

            group.MapGet("/{id:int}", async (HttpContext context, int id) => {
                var product = products.FirstOrDefault(x => x.Id == id);

                if (product is null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Incorrect Product Id");
                    return;
                }

                await context.Response.WriteAsync(JsonSerializer.Serialize(product.ToString()));
            });

            group.MapPost("/", async (HttpContext context, Product product) =>
            {
                products.Add(product);
                await context.Response.WriteAsync("Product added");
            });

            group.MapPut("/{id:int}", async (HttpContext context, int id, [FromBody] Product productDto) =>
            {
                var product = products.FirstOrDefault(x => x.Id == id);

                if(product is null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Incorrect Product Id");
                    return;
                }

                product.ProductName = productDto.ProductName;

                await context.Response.WriteAsync("Product updated");
            });

            group.MapDelete("/{id:int}", async (HttpContext context, int id) =>
            {
                var product = products.FirstOrDefault(x => x.Id == id);

                if (product is null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Incorrect Product Id");
                    return;
                }

                products.Remove(product);

                await context.Response.WriteAsync("Product deleted");
            });

            return group;
        }
    }
}
