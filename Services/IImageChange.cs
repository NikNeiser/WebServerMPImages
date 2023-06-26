

namespace WebServerMPImages.Services
{
    using WebServerMPImages.Models;
    using WebServerMPImages.Models.Images;
    public interface IImageChange
    {
        public Task<IEnumerable<string>> ProcessImages(IEnumerable<ImageInputModel> images);
        public void RemoveImages(IEnumerable<string> photoToDelete);
        public string ChangeTempName(string name);       

    }
}
