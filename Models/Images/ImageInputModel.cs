namespace WebServerMPImages.Models.Images
{
    public class ImageInputModel
    {
        
        public string Name { get; set; }
        public string Type { get; set; }
        public Stream Content { get; set; }
    }
}
