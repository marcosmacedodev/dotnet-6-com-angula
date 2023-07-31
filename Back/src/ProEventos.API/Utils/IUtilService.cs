using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.API.Utils
{
    public interface IUtilService
    {
        Task<string> SaveImage(IFormFile imageFile, string folder);
        bool DeleteImage(string fileName, string folder);
    }
}