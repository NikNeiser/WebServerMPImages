
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

    public class ImagesController : Controller
    {
        private readonly IImageService imageService;
        private readonly AppDbContext _db;

        public ImagesController(AppDbContext db, IImageService imageService)
        {
            _db = db;
            this.imageService = imageService;
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
        public IActionResult IndexPost(IEnumerable<string> imageNames, ImageParametersPreset preset)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPresets() => Ok( await _db.Presets.ToListAsync());



    }
}
