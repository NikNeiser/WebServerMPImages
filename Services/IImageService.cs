

namespace WebServerMPImages.Services
{
    using WebServerMPImages.Models.Images;
    public interface IImageService
    {
        public IEnumerable<string> ProcessImages(IEnumerable<ImageInputModel> images);
        public void RemoveImages(IEnumerable<string> photoToDelete);
        public string ChangeTempName(string name);
    }
}
