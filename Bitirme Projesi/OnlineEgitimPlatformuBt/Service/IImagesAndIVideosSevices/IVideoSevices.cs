using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IImagesAndIVideosSevices
{
    public interface IVideoSevices
    {
        void DeleteVideo(string VideoUrl);

        string CreateVideo(IFormFile Video);

        string UpdateVideo(string? VideoUrl, IFormFile? Video);

    }
}
