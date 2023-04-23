namespace WebServerMPImages.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum BrandType
    {
        BA = 0,
        SAN = 1,
        ENERGY = 2,
        NONE = 3
    }
    public class Brand
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public BrandType BrandType { get; set; }

    }
}
