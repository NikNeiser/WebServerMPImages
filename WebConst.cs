namespace WebServerMPImages
{
    public static class WebConst
    {
        public static string imagePath = @"/Content/Images/";
        public static string previewImagePath = @"/Content/Preview/";
        public static string previewImageFormat = ".webp";
        public static float threshold = 0.5F;
        public static string tmpPrefix = "tmp_";
        public static IEnumerable<string> ImagePaths { 
            get {
                yield return imagePath;
                yield return previewImagePath;
            }
        }
    }
}
