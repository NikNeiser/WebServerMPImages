namespace WebServerMPImages.Models.ViewModels
{
    using WebServerMPImages.Models;
    public class FullDbVM
    {
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
