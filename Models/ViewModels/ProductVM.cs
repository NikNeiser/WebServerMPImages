using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebServerMPImages.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> BrandsDropDown { get; set; }

    }
}
