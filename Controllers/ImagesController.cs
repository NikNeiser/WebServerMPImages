
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
            ImageParametersPreset preset = new ImageParametersPreset();

            preset.PresetName = HttpContext.Request.Form["PresetName"];
            preset.Width = int.Parse(HttpContext.Request.Form["Width"]);
            preset.Height = int.Parse(HttpContext.Request.Form["Height"]);
            preset.TransparentBG = !HttpContext.Request.Form["TransparentBG"].IsNullOrEmpty();
            preset.BGColor = HttpContext.Request.Form["BGColor"];
            preset.NameByBarcode = !HttpContext.Request.Form["NameByBarcode"].IsNullOrEmpty();
            preset.Extension = (ImageExtension)int.Parse(HttpContext.Request.Form["Extension"]);
            

            imageGetService.GetImages(images, preset);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPresets() => Ok( await _db.Presets.ToListAsync());



    }
}
