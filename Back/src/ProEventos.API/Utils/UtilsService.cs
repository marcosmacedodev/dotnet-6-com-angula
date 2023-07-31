using System.Security.Claims;

namespace ProEventos.API.Utils
{
    public class UtilsService: IUtilsService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public UtilsService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public bool DeleteImage(string fileName, string folder)
        {
            if(fileName == null || folder == null) return false;
            string path = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources\{folder}", fileName);
            if (File.Exists(path)){
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<string> SaveImage(IFormFile imageFile, string folder){
            
            string fileName = new string( Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');

            fileName = $"{fileName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            string fullPath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources\{folder}");

            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

            fullPath = Path.Combine(fullPath, fileName);

            using(var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}