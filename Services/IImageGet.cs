using WebServerMPImages.Models;

namespace WebServerMPImages.Services
{
    public interface IImageGet
    {
        public string GetImages(IEnumerable<string> images, ImageParametersPreset preset);
    }
}
