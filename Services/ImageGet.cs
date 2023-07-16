using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using WebServerMPImages.Data;
using WebServerMPImages.Models;

namespace WebServerMPImages.Services
{
    public class ImageGet : IImageGet
    {
        private string _queryName = Guid.NewGuid().ToString();
        private DirectoryInfo? _di;
        private readonly string wwwrootpath;
        private List<String> _names = new List<string>();

        public ImageGet(IWebHostEnvironment webHostEnviroment)
        {
            wwwrootpath = webHostEnviroment.WebRootPath;
            CreateDirectory();
        }
        private void CreateDirectory()
        {            
            _di = Directory.CreateDirectory(String.Concat(wwwrootpath, WebConst.downloadPath, _queryName));
        }

        public async Task<string> GetImages(IEnumerable<ProductPhoto> images, ImageParametersPreset preset)
        {
            await CreateImages(images, preset);
            return MakeZipArchive();
        }
        private async Task CreateImages(IEnumerable<ProductPhoto> images, ImageParametersPreset preset)
        {
            var tasks = images.Select(image => Task.Run(async () => await CreateImage(image, preset)));
            await Task.WhenAll(tasks);
        }

        private async Task CreateImage(ProductPhoto photo, ImageParametersPreset preset)
        {
            string imagePath = String.Concat(wwwrootpath, WebConst.imagePath, photo.Name, WebConst.previewImageFormat);
            using var image = await Image.LoadAsync(imagePath);

            image.Mutate(i => i.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Pad,
                PremultiplyAlpha = true,
                Size = new Size(preset.Width - 2 * preset.Padding, preset.Height - 2 * preset.Padding),
            }));

            if(preset.Padding>0)
            {
                image.Mutate(i => i.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.BoxPad,
                    PremultiplyAlpha = true,
                    Size = new Size(preset.Width, preset.Height),
                }));
            }

            if(!preset.TransparentBG)
                image.Mutate(i => i.BackgroundColor(Color.Parse(preset.BGColor)));            

            string imageSaveName = GetImageSaveName(photo, preset.NameByBarcode);
            string imageSavePath = Path.Combine(_di.FullName, imageSaveName);

            switch (preset.Extension)
            {
                case ImageExtension.png:
                    await image.SaveAsPngAsync(imageSavePath+".png");
                    break;
                case ImageExtension.jpg:
                    await image.SaveAsJpegAsync(imageSavePath+".jpg");
                    break;
            }
        }

        private string GetImageSaveName(ProductPhoto prodPhoto, bool nameByBarcode)
        {
            string saveName;            

            if (nameByBarcode)
            {
                saveName = prodPhoto.Product.Barcode;
            }
            else
            {
                saveName = $"{prodPhoto.Product.Brand.Name}_" +
                    $"{prodPhoto.Product.Taste}_" +
                    $"{prodPhoto.Product.PackType}_" +
                    $"{prodPhoto.Product.Volume}_" +
                    $"{prodPhoto.PhotoType}";
            }
            string index = GetIndex(saveName);
            saveName = String.Concat(saveName, index);
            _names.Add(saveName);
            return saveName;
        }

        private string GetIndex(string saveName)
        {
            int filesWithSameNameCount = _names.Where(i => i.Contains(saveName)).Count();
            filesWithSameNameCount++;
            return filesWithSameNameCount > 1 ?
                String.Concat("_",filesWithSameNameCount.ToString()) :
                String.Empty;
        }

        private string MakeZipArchive()
        {
            var archivePath = String.Concat(wwwrootpath, WebConst.downloadPath, _queryName, ".zip");            
            ZipFile.CreateFromDirectory(_di.FullName, archivePath);            
            _di.Delete(true);
            archivePath = String.Concat(WebConst.downloadPath, _queryName, ".zip");
            return archivePath;
        }

    }
}
