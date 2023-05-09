using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServerMPImages.Models
{
    using System.Drawing;
    public class ImageParametersPreset
    {
        [NotMapped]
        private Color backgroundColor = Color.White;

        [Key]
        public string PresetName { get; set; }
        [NotMapped]
        public Size Size => new Size(Width,Height);
        public int Width { get; set; }
        public int Height { get; set; }
        public bool NameByBarcode { get; set; } = false;
        public bool TransparentBG { get; set; } = true;
        [NotMapped]
        public Color BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public string BGColor { get => backgroundColor.ToHex(); set => backgroundColor = ColorTranslator.FromHtml(value); }
        public ImageExtension Extension { get; set; } = ImageExtension.png;

        public ImageParametersPreset()
        {
            PresetName = "None";
        }
    }

}
