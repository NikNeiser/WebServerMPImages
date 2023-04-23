namespace WebServerMPImages.Models.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    public class BrandVM
    {
        public Brand Brand { get; set; }
        public IEnumerable<SelectListItem> BrandTypeDropDown { get; set; } =
            Enum.GetValues(typeof(BrandType)).Cast<BrandType>().
            Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
    }
}
