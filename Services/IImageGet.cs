using WebServerMPImages.Models;

namespace WebServerMPImages.Services
{
    public interface IImageGet
    {
        public Task<string> GetImages(IEnumerable<ProductPhoto> images, ImageParametersPreset preset);
    }
}
