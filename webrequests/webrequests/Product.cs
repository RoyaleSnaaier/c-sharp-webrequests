namespace Webrequest
{
    public class Product
    {
        public Product() { }

        public Product(string name, decimal price, string shortDesc, ProductType productType)
        {
            Name = name;
            Price = price;
            ShortDescription = shortDesc;
            ProductType = productType;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public ProductType ProductType { get; set; }
    }
}