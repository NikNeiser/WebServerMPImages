using WebServerMPImages.Models.Images;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using WebServerMPImages.Models;

namespace WebServerMPImages.Services
{    
    public class ImageChange : IImageChange
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly string wwwrootpath; 
        public ImageChange(IWebHostEnvironment webHostEnviroment)
        {
            this.webHostEnviroment = webHostEnviroment;
            wwwrootpath = webHostEnviroment.WebRootPath;
        }

        public async Task<IEnumerable<string>> ProcessImages(IEnumerable<ImageInputModel> images)
        {   
            List<string> result = new List<string>();

            foreach (var image in images)
            {
                result.Add(await SaveImage(image));
            }
            return result;
        }

        private async Task<string> SaveImage(ImageInputModel imageInput)
        {
            string imageName = GetTempName();
            using var image = await Image.LoadAsync(imageInput.Content);
            image.Mutate(i => i.EntropyCrop(WebConst.threshold));
            image.Metadata.ExifProfile = null;
            await image.SaveAsWebpAsync(GetImageOriginalPath(imageName));

            image.Mutate(i => i.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Pad,
                PremultiplyAlpha = true,
                Size = WebConst.previewImageSize,
            }));

            await image.SaveAsWebpAsync(GetImagePreviewPath(imageName));
            return imageName;
        }

        public void RemoveImages(IEnumerable<string> photoToDelete)
        {
            foreach (var photo in photoToDelete)
            {
                DeleteIfExist(GetImagePreviewPath(photo));
                DeleteIfExist(GetImageOriginalPath(photo));
            }
        }

        private void DeleteIfExist(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string ChangeTempName(string tempName)
        {
            string name = tempName;

            if (tempName.StartsWith(WebConst.tmpPrefix))
            {
                name = tempName.Substring(WebConst.tmpPrefix.Length);
                RenameImage(tempName,name);
            }
            return name;
        }
        private void RenameImage(string oldName, string newName)
        {
            foreach (var path in WebConst.ImagePaths)
            {
                File.Move(GetImagePath(path,oldName), GetImagePath(path, newName));
            }
        }

        private string GetTempName()
        {
            return String.Concat(WebConst.tmpPrefix, Guid.NewGuid().ToString());
        }
        private string GetImagePreviewPath(string name)
        {
            return GetImagePath(WebConst.previewImagePath, name);
        }

        private string GetImageOriginalPath(string name)
        {
            return GetImagePath(WebConst.imagePath, name);
        }

        private string GetImagePath(string path, string name)
        {
            return String.Concat(wwwrootpath, path, name, WebConst.previewImageFormat);
        }

        public string GetImages(IEnumerable<string> images, ImageParametersPreset preset)
        {
            throw new NotImplementedException();
        }

        //private void SaveImageImageSharp(ImageInputModel imageInput)
        //{
        //    int padding = 20;

        //    using var image = Image.Load(imageInput.Content);
        //    image.Mutate(i => i.EntropyCrop(WebConst.threshold));
        //    image.Metadata.ExifProfile = null;
        //    image.SaveAsWebp($"{WebConst.ImagePath}{imageInput.Name}.webp");


        //    image.Mutate(i => i.Resize(new ResizeOptions
        //    {
        //        Mode = ResizeMode.Pad,
        //        PremultiplyAlpha = true,
        //        Size = new Size(1160,1560),
        //    }));

        //    image.Mutate(i => i.Resize(new ResizeOptions
        //    {
        //        Mode = ResizeMode.BoxPad,
        //        PremultiplyAlpha = true,
        //        Size = new Size(1200, 1600),
        //    }));

        //    image.SaveAsPng($"{WebConst.ImagePath}{imageInput.Name}.png");
        //    image.Mutate(i => i.BackgroundColor(Color.White));
        //    image.SaveAsJpeg($"{WebConst.ImagePath}{imageInput.Name}.jpg");
        //}


        //private void SaveImageImageprocessor(ImageInputModel image)
        //{
        //    Stream inStream = image.Content;
        //    ImageFactory imageFactory = new ImageFactory(preserveExifData: true);

        //    imageFactory.Load(inStream);
        //    imageFactory.EntropyCrop(WebConst.threshold).Resize(new System.Drawing.Size { Width = 1200, Height = 1600 });

        //    foreach (var format in formats)
        //    {
        //        Stream outStream = new FileStream($"{WebConst.ImagePath}{image.Name}.{format.DefaultExtension}", FileMode.Create, FileAccess.Write);

        //        if (format is JpegFormat)
        //            imageFactory.BackgroundColor(System.Drawing.Color.White);
        //        else
        //            imageFactory.BackgroundColor(System.Drawing.Color.Transparent);

        //        imageFactory.Format(format).Save(outStream);                        
        //    }
        //}


    }
}
