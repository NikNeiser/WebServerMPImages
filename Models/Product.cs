namespace WebServerMPImages.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Barcode { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string? Taste { get; set; }
        public PackType PackType { get; set; }
        public int Volume { get; set; }
        public virtual List<ProductPhoto>? Photos { get; set; }
        
    }
}
