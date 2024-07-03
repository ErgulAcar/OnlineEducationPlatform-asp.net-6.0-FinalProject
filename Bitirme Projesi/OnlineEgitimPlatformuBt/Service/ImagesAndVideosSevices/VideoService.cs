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
    public class VideoService : IVideoSevices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideoService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }



        public string CreateVideo(IFormFile Video)
        {
            if (Video != null && Video.Length > 0)
            {

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(Video.FileName)}";

                string path = Path.Combine("wwwroot", "VideosAndImages" + "/Video", randomFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    Video.CopyTo(fileStream);
                }

                return randomFileName;

            }

            return null;
        }



        public void DeleteVideo(string VideoUrl)
        {
            if (VideoUrl != null)
            {
                var newVideoPath = Path.Combine("wwwroot", "VideosAndImages", "Video", VideoUrl);

                FileInfo fi = new FileInfo(newVideoPath);

                System.IO.File.Delete(VideoUrl);

                fi.Delete();
            }
        }



        public string UpdateVideo(string? VideoUrl, IFormFile? Video)
        {
            if (VideoUrl != null)
            {
                var newVideoPath = Path.Combine("wwwroot", "VideosAndImages", "Video", VideoUrl);

                FileInfo fi = new FileInfo(newVideoPath);

                System.IO.File.Delete(VideoUrl);

                fi.Delete();
            }

            if ((Video != null))
            {
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(Video.FileName)}";

                string path = Path.Combine("wwwroot", "VideosAndImages" + "/Video", randomFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    Video.CopyTo(fileStream);
                }
                return randomFileName;
            }


            return null;
        }


    }
}
