using DotNet6_Project.Models;
using DotNet6_Project.Services;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers(); 
// Removed in .NET 9 in favor of Minimal APIs
builder.Services.AddAuthorization(); // Required if you're using app.UseAuthorization()

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// app.MapControllers(); 
// Replaced with Minimal APIs below

// Minimal API for GET all products
app.MapGet("/api/products", (IProductService service) =>
{
    return TypedResults.Ok(service.GetAll());
})
.WithName("GetAllProducts")
.WithSummary("Returns all products")
.WithOpenApi();

// Minimal API for GET by ID
app.MapGet("/api/products/{id}", async (HttpContext context, int id) =>
{
    var service = context.RequestServices.GetService<IProductService>();
    var product = service?.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// Minimal API for POST (Create)
app.MapPost("/api/products", (Product product, IProductService service) =>
{
    service.Add(product);
    return TypedResults.Created($"/api/products/{product.Id}", product);
})
.WithName("CreateProduct")
.WithSummary("Adds a new product")
.WithOpenApi();

// Minimal API for PUT (Update)
app.MapPut("/api/products/{id}", (HttpContext context, int id, Product product) =>
{
    var service = context.RequestServices.GetService<IProductService>();

    if (id != product.Id)
        return Results.BadRequest();

    service?.Update(product);
    return Results.NoContent();
})
.WithName("UpdateProduct")
.WithSummary("Updates a product")
.WithOpenApi();

// Minimal API for DELETE
app.MapDelete("/api/products/{id}", (int id, IProductService service) =>
{
    service.Delete(id);
    return TypedResults.NoContent();
})
.WithName("DeleteProduct")
.WithSummary("Deletes a product")
.WithOpenApi();

app.Run();
