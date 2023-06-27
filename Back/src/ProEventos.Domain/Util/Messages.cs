using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain.Util
{
    public class Messages
    {
        public const string REQUIRED = "O campo {0} é obrigatório.";
        public const string EMAILADDRESS = "O campo {0} deve conter um e-mail válido.";
        public const string MINLENGTH = "O campo {0} deve conter no mínimo {1} caracteres";
        public const string MAXLENGTH = "O campo {0} deve conter no máximo {1} caracteres";
        public const string RANGE = "O campo {0} deve conter entre {1} e {2}";
        public const string PHONE = "O campo {0} deve conter um número válido";
        public const string REIMGFORMAT = "O campo {0} deve conter um URL com formato de imagem válido. (GIF, JPG ou JPEG, BMP, PNG)";
    }
}