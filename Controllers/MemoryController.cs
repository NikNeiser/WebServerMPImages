using Microsoft.AspNetCore.Mvc;
using WebServerMPImages.Services;
using WebServerMPImages.Data;
using WebServerMPImages.Models;
using WebServerMPImages.Models.ViewModels;

namespace WebServerMPImages.Controllers
{
    public class MemoryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IImageChange _imageService;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly string wwwrootpath;
        public MemoryController(AppDbContext db, IImageChange imageService, IWebHostEnvironment webHostEnviroment)
        {
            _db = db;
            _imageService = imageService;
            _webHostEnviroment = webHostEnviroment;
            wwwrootpath = webHostEnviroment.WebRootPath;

        }
        public IActionResult Index()
        {
            MemoryVM memoryVM = new MemoryVM();

            DirectoryInfo di = new DirectoryInfo(String.Concat(wwwrootpath,WebConst.imagePath.TrimEnd('/')));
            memoryVM.ImagesInDir = di.GetFiles().Length;
            memoryVM.ImagesSize =
                (((float)di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length))
                /(1024 * 1024))
                .ToString("0.00")
                +" Mb";

            memoryVM.ImagesInDB = _db.ProductPhoto.Count();

            return View(memoryVM);
        }

        public IActionResult Delete()
        {
            DirectoryInfo di = new DirectoryInfo(String.Concat(wwwrootpath, WebConst.imagePath.TrimEnd('/')));
            var imagesInDir = di.GetFiles().Select(i => Path.GetFileNameWithoutExtension(i.Name));
            var imagesInDB = _db.ProductPhoto.Select(i => i.Name);

            var imagesToDelete = imagesInDir.Except(imagesInDB);

            _imageService.RemoveImages(imagesToDelete);

            return RedirectToAction(nameof(this.Index));
        }
    }
}
