using Microsoft.AspNetCore.Mvc;
using MinimalAPI.EndpointFilters;
using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;
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
                    return Results.ValidationProblem(new Dictionary<string, string[]> {
                        { "id", new string[] { "Incorrect Product Id" } }
                    });
                }

                return Results.Ok(new { message = product.ToString() });
            });

            group.MapPost("/", async (HttpContext context, Product product) =>
            {
                products.Add(product);
                return Results.Ok(new { message = "Product added" });
            })
                .AddEndpointFilter<CustomEndpointFilter>()
                .AddEndpointFilter(async (context, next) => 
                {
                    var product = context.Arguments.OfType<Product>().FirstOrDefault();

                    if (product is not null)
                    {
                        return Results.BadRequest("Product details are not found in the request");
                    }

                    var validationContext = new ValidationContext(product);

                    var errors = new List<ValidationResult>();
                    bool isValid = Validator.TryValidateObject(product, validationContext, errors, true);

                    if (!isValid)
                    {
                        return Results.BadRequest(errors.FirstOrDefault()?.ErrorMessage);
                    }

                    //Before logic

                    var result = await next(context);

                    //After logic
                    return result;
                });

            group.MapPut("/{id:int}", async (HttpContext context, int id, [FromBody] Product productDto) =>
            {
                var product = products.FirstOrDefault(x => x.Id == id);

                if(product is null)
                {
                    return Results.BadRequest(new { error = "Incorrect Product Id" });
                }

                product.ProductName = productDto.ProductName;

                return Results.Ok( new { message = "Product updated" });
            });

            group.MapDelete("/{id:int}", async (HttpContext context, int id) =>
            {
                var product = products.FirstOrDefault(x => x.Id == id);

                if (product is null)
                {
                    return Results.BadRequest(new { error = "Incorrect Product Id" });
                }

                products.Remove(product);

                return Results.Ok( new { message = "Product deleted" });
            });

            return group;
        }
    }
}
