namespace WebServerMPImages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebServerMPImages.Services;
    using WebServerMPImages.Data;
    using WebServerMPImages.Models.ViewModels;
    using WebServerMPImages.Models;
    using System.Linq;

    public class BrandController : Controller
    {
        private readonly AppDbContext _db;

        public BrandController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Upsert(int? Id)
        {
            Brand brand = new Brand();

            if (Id > 0)
            {
                brand = _db.Brands.Find(Id);
            }      

            return View(brand);
        }

        [HttpPost]
        public IActionResult Upsert(Brand brand)
        {
            if(brand.Id == 0)
            {
                _db.Brands.Add(brand);
            }
            else
            {
                _db.Brands.Update(brand);
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(ProductController.Index), nameof(ProductController).Replace("Controller",String.Empty));
        }

        public IActionResult Delete(int Id)
        {
            Brand brand = _db.Brands.Find(Id);
            return View(brand);
        }

        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            _db.Brands.Remove(brand);
            _db.SaveChanges();
            return RedirectToAction(nameof(ProductController.Index), nameof(ProductController).Replace("Controller",String.Empty));
        }

    }
}
