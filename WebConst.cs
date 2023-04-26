namespace WebServerMPImages
{
    public static class WebConst
    {
        public const string imagePath = @"/Content/Images/";
        public const string previewImagePath = @"/Content/Preview/";
        public const string previewImageFormat = ".webp";
        public const float threshold = 0.5F;
        public const string tmpPrefix = "tmp_";
        public static Size previewImageSize = new Size(200, 300);
        public static IEnumerable<string> ImagePaths { 
            get {
                yield return imagePath;
                yield return previewImagePath;
            }
        }
    }
}
