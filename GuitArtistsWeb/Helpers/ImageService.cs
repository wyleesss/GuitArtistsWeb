namespace GuitArtistsWeb.Helpers
{
    public class ImageService
    {
        private readonly string _imageFolderPath;

        public ImageService(string imageFolderPath)
        {
            _imageFolderPath = imageFolderPath;
        }

        public string? SaveImage(IFormFile imageFile, Guid lessonID)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;
            if (!IsImageFile(imageFile.FileName))
                throw new ArgumentException("It is not image");

            var uniqueFileName = lessonID.ToString() + Path.GetExtension(imageFile.FileName);

            var filePath = Path.Combine(_imageFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return filePath;
        }

        public bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp";
        }
    }
}
