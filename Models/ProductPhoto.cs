namespace WebServerMPImages.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProductPhoto
    {
        [Key]
        public string Name { get; set; }

        public PhotoType PhotoType { get; set; }

        public Product Product { get; set; }
    }
}
