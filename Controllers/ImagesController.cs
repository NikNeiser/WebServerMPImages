
namespace WebServerMPImages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebServerMPImages.Services;
    using WebServerMPImages.Data;
    using WebServerMPImages.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using WebServerMPImages.Models.ViewModels;
    using System.Text.Json;
    using Microsoft.IdentityModel.Tokens;

    public class ImagesController : Controller
    {
        private readonly IImageGet imageGetService;
        private readonly AppDbContext _db;

        public ImagesController(AppDbContext db, IImageGet imageGetService)
        {
            _db = db;
            this.imageGetService = imageGetService;
        }

        public IActionResult Index()
        {
            var imagesVM = new ImagesVM
            {
                ImagesGroup = _db.Products.Include(u => u.Brand).Include(u => u.Photos.OrderBy(p => p.PhotoType)).GroupBy(u => u.Brand),
                Presets = JsonSerializer.Serialize(_db.Presets.ToList())
            }; 
             return View(imagesVM);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<string> images)
        {
            ImageParametersPreset preset = new ImageParametersPreset() 
            {
                PresetName = HttpContext.Request.Form["PresetName"],
                Width = int.Parse(HttpContext.Request.Form["Width"]),
                Height = int.Parse(HttpContext.Request.Form["Height"]),
                Padding = int.Parse(HttpContext.Request.Form["Padding"]),
                TransparentBG = !HttpContext.Request.Form["TransparentBG"].IsNullOrEmpty(),
                BGColor = HttpContext.Request.Form["BGColor"],
                NameByBarcode = !HttpContext.Request.Form["NameByBarcode"].IsNullOrEmpty(),
                Extension = (ImageExtension)int.Parse(HttpContext.Request.Form["Extension"]),            
            };            

            imageGetService.GetImages(images, preset);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPresets() => Ok( await _db.Presets.ToListAsync());



    }
}
