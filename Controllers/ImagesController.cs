
namespace WebServerMPImages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebServerMPImages.Services;
    using WebServerMPImages.Data;
    using WebServerMPImages.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

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
            var productsGroup = _db.Products.Include(u=> u.Brand).Include(u => u.Photos.OrderBy(p => p.PhotoType)).GroupBy(u => u.Brand);
            return View(productsGroup);
        }


    }
}
