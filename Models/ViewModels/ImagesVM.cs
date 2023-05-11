namespace WebServerMPImages.Models.ViewModels
{
    public class ImagesVM
    {
        public IQueryable<IGrouping<Brand, Product>> ImagesGroup { get; set; }
        public string Presets { get; set; }
    }
}
