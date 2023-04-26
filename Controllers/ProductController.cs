namespace WebServerMPImages.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebServerMPImages.Services;
    using WebServerMPImages.Data;
    using WebServerMPImages.Models;
    using WebServerMPImages.Models.ViewModels;
    using System.Linq;
	using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Text.Json;
    using System.Reflection;

    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IImageService _imageService;

        public ProductController(AppDbContext db, IImageService imageService)
        {
            _db = db;
            _imageService = imageService;

        }

        public async Task<IActionResult> Index()
        {
            FullDbVM fullDbVM = new FullDbVM
            {
                Brands = _db.Brands,
                //Products = from p in _db.Products
                //           orderby p.BrandId, p.Taste, p.Volume
                //           select p
                Products = await _db.Products.Include(u => u.Photos.OrderBy(p => p.PhotoType)).OrderBy(u => u.BrandId).ToListAsync()
            };
            return View(fullDbVM);
        }

        public IActionResult Upsert(int? Id)
        {
            Product product = Id > 0 ?
                _db.Products.Include(u => u.Photos.OrderBy(p => p.PhotoType)).FirstOrDefault(u => u.Id == Id) ?? new Product() : 
                new Product();

            ProductVM productVM = new ProductVM
            {
                Product = product,
                BrandsDropDown = _db.Brands.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM productVM, IEnumerable<string>? allPhotos, IEnumerable<string>? photoToDelete)
        {

            //var newPhotos = new List<ProductPhoto>();

            //foreach (string photo in allPhotos.OrEmptyIfNull())
            //{
            //    newPhotos.Add(JsonSerializer.Deserialize<ProductPhoto>(photo));
            //}

            List<ProductPhoto> newPhotos = allPhotos.Select(i => JsonSerializer.Deserialize<ProductPhoto>(i)).ToList() ?? new List<ProductPhoto>();

            if (productVM.Product.Id == 0)
            {
                newPhotos.ForEach(i => i.Name = _imageService.ChangeTempName(i.Name));
                productVM.Product.Photos = newPhotos;
                _db.Products.Add(productVM.Product);
            }
            else
            {
                var currentProduct = _db.Products.Include(u => u.Photos.OrderBy(p => p.PhotoType))
                    .FirstOrDefault(i => i.Id == productVM.Product.Id);

                if (currentProduct.Photos == null)
                    currentProduct.Photos = new List<ProductPhoto>();

                List<ProductPhoto> currentPhotos = currentProduct.Photos;

                //Update product properties
                Type type = currentProduct.GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    property.SetValue(currentProduct, property.GetValue(productVM.Product) ?? property.GetValue(currentProduct));
                }


                foreach(var p in newPhotos.EmptyIfNull())
                {
                    if(currentPhotos.Any(i => i.Name == p.Name))
                    {
                        currentPhotos.Single(i => i.Name == p.Name).PhotoType = p.PhotoType;
                    }
                    else
                    {
                        p.Name = _imageService.ChangeTempName(p.Name);
                        currentPhotos.Add(p);
                    }
                }

                foreach(var p in photoToDelete.EmptyIfNull())
                {
                    if (currentPhotos.Any(i => i.Name == p))
                    {
                        _db.ProductPhoto.Remove(currentPhotos.Single(i => i.Name == p));
                        currentPhotos.Remove(currentPhotos.Single(i => i.Name == p));                        
                    }
                }                
            }

            await _db.SaveChangesAsync();

            if (photoToDelete!=null && photoToDelete.Count()>0)
                _imageService.RemoveImages(photoToDelete);

            return RedirectToAction(nameof(this.Index));
            
        }
                
        public IActionResult Delete(int? Id)
        {
            var product = _db.Products.Include(u => u.Photos.OrderBy(p => p.PhotoType)).FirstOrDefault(i => i.Id == Id);

            if (product == null) return NotFound();

            var currentPhotos = product.Photos;

            foreach(var p in currentPhotos.EmptyIfNull())
            {
                _db.ProductPhoto.Remove(p);
            }
            _imageService.RemoveImages(currentPhotos.Select(i => i.Name));

            _db.Products.Remove(product);
            _db.SaveChanges();

            return RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadNewImages(IFormFile[] images)
        {
            if (images.Length > 0)
            {
                IEnumerable<string> newImages = await this._imageService
                    .ProcessImages(images.Select(i => new Models.Images.ImageInputModel
                    {
                        Name = Path.GetFileNameWithoutExtension(i.FileName),
                        Type = i.ContentType,
                        Content = i.OpenReadStream()
                    }));

                List<ProductPhoto> newProdPhotos = new List<ProductPhoto>();

                foreach (var image in newImages)
                {
                    newProdPhotos.Add(new ProductPhoto
                    {
                        Name = image,
                        PhotoType = PhotoType.None
                    });
                }

                return PartialView("_UploadetPhoto", newProdPhotos);
            }
            return NotFound();
        }
    }
}
