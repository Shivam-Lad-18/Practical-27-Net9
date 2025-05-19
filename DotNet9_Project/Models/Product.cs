namespace DotNet9_Project.Models
{
    public class Product
    {
        //'required' modifier for non-nullable properties
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Category { get; set; }
        // feild keyword for property backing field
        public decimal Discount
        {
            get;
            set
            {
                field = value < 0 ? 0 : value; 
            }
        }
    }
}
