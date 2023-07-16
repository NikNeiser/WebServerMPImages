using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServerMPImages.Models
{
    using System.Drawing;
    using System.Text.Json.Serialization;

    public class ImageParametersPreset
    {
        [NotMapped]
        [JsonIgnore]
        private Color backgroundColor = Color.White;

        [Key]        
        public string PresetName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Padding { get; set; } = 0;
        public bool NameByBarcode { get; set; } = false;
        public bool TransparentBG { get; set; } = true;
        [NotMapped]
        [JsonIgnore]
        public Color BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public string BGColor { get => backgroundColor.ToHex(); set => backgroundColor = ColorTranslator.FromHtml(value); }
        public ImageExtension Extension { get; set; } = ImageExtension.png;

        public ImageParametersPreset()
        {
            PresetName = "None";
        }
    }

}
