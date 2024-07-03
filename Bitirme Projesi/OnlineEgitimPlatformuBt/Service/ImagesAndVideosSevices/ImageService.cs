using Core.IImagesAndIVideosSevices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Service.ImagesAndVideosSevices
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public string CreatePicture(IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(Image.FileName)}";

                string path = Path.Combine("wwwroot", "VideosAndImages", "Images", randomFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                return randomFileName;

            }

            return null;
        }


        public void DeletePicture(string ResimUrl)
        {
            if (ResimUrl != null)
            {
                var newPicturePath = Path.Combine("wwwroot", "VideosAndImages", "Images", ResimUrl);

                FileInfo fi = new FileInfo(newPicturePath);

                System.IO.File.Delete(ResimUrl);

                fi.Delete();
            }
        }



        public string UpdatePicture(string? ResimUrl, IFormFile? Image)
        {
            if (ResimUrl != null)
            {
                var newPicturePath = Path.Combine("wwwroot", "VideosAndImages", "Images", ResimUrl);

                FileInfo fi = new FileInfo(newPicturePath);

                System.IO.File.Delete(ResimUrl);

                fi.Delete();
            }

            if ((Image != null))
            {
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(Image.FileName)}";

                string path = Path.Combine("wwwroot", "VideosAndImages", "Images", randomFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
                return randomFileName;
            }


            return null;
        }

    }
}
