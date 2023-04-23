using SixLabors.ImageSharp;

namespace WebServerMPImages.Services
{
    using WebServerMPImages.Models.Images;
    //using ImageProcessor;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    //using ImageProcessor.Imaging.Formats;
    //using ImageProcessor.Plugins.WebP.Imaging.Formats;


    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly string wwwrootpath; 
        public ImageService(IWebHostEnvironment webHostEnviroment)
        {
            this.webHostEnviroment = webHostEnviroment;
            wwwrootpath = webHostEnviroment.WebRootPath;
        }

        public IEnumerable<string> ProcessImages(IEnumerable<ImageInputModel> images)
        {   
            List<string> result = new List<string>();

            foreach (var image in images)
            {
                result.Add(SaveImageImageSharp(image));
            }
            return result;
        }

        private string SaveImageImageSharp(ImageInputModel imageInput)
        {
            string imageName = GetTempName();
            using var image = Image.Load(imageInput.Content);
            image.Mutate(i => i.EntropyCrop(WebConst.threshold));
            image.Metadata.ExifProfile = null;
            image.SaveAsWebp(GetImageOriginalPath(imageName));


            image.Mutate(i => i.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Pad,
                PremultiplyAlpha = true,
                Size = new Size(200, 300),
            }));

            image.SaveAsWebp(GetImagePreviewPath(imageName));
            return imageName;
        }

        public void RemoveImages(IEnumerable<string> photoToDelete)
        {
            foreach (var photo in photoToDelete)
            {
                if (File.Exists(GetImagePreviewPath(photo)))
                {
                    File.Delete(GetImagePreviewPath(photo));
                }

                if (File.Exists(GetImageOriginalPath(photo)))
                {
                    File.Delete(GetImageOriginalPath(photo));
                }
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
