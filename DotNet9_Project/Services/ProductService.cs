using DotNet9_Project.Models;
using System.Xml.Linq;

namespace DotNet9_Project.Services
{
    public class ProductService : IProductService
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200, Category = "Electronics" },
            new Product { Id = 2, Name = "Smartphone", Price = 800, Category = "Electronics" },
            new Product { Id = 3, Name = "Table", Price = 150, Category = "Furniture" },
            new Product { Id = 4, Name = "Chair", Price = 75, Category = "Furniture" },
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Category = product.Category;
                existing.Discount = product.Discount;
            }
        }

        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
                _products.Remove(product);
        }

        // Demonstrates 'params' collections - new in .NET 9 / C# 13
        // Accepts any number of filters as Func<Product,bool> delegates
        public IEnumerable<Product> FilterProducts(params Func<Product, bool>[] filters)
        {
            // Use LINQ All to apply all filters
            return _products.Where(p => filters.All(f => f(p)));
        }
    }
}
