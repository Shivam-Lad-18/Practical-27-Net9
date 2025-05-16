namespace DotNet9_Project.Models
{
    public class Product
    {
        // New C# 11 feature: 'required' modifier for non-nullable properties
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Category { get; set; }

        // Using 'field' keyword - New in C# 13 (part of field-targeted attributes or expressions)
        // Example: Demonstrate 'field' keyword in property setter (dummy use here)
        public decimal Discount
        {
            get;
            set
            {
                field = value < 0 ? 0 : value; // ✅ Valid: used in auto-property setter
            }
        }
    }
}
