using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IImagesAndIVideosSevices
{
    public interface IImageService
    {
        void DeletePicture(string ResimUrl);

        string CreatePicture(IFormFile Image);

        string UpdatePicture(string? ResimUrl, IFormFile? Image);

    }
}
