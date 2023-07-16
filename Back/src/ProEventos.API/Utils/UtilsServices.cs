using System.Security.Claims;

namespace ProEventos.API.Utils
{
    public class UtilsServices
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UtilsServices(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
        }

        public string GetUserName(){
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        }

        public int GetUserId(){
            int id;
            string idstr;
            idstr = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idstr, out id)? id: 0;
        }

        public bool DeleteImage(string path){
            path = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources\Images", path);
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

        public async Task<string> SaveImage(IFormFile imageFile){
            
            string path = new string( Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');

            path = $"{path}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            string fullPath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources\Images", path);

            using(var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return path;
        }
    }
}