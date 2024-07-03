using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace OnlineEğitimPlatformuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SepetController : ControllerBase
    {
        private IMemoryCache _memoryCache;

        public SepetController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        //listeleme
        [HttpGet]
        public IActionResult List()
        {
            //MemoryCache oluşturuyoruz
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //süreyi 2 saat olarak ayarlıyoruz
            options.SlidingExpiration = TimeSpan.FromHours(2);

            List<Sepet> sepet = new List<Sepet>();

            for (int i = 0; i < 20; i++)
            {
                //burda tüm sepetler için ıdyi 0dan başlatıyoruz
                var data = i.ToString();
                //_memoryCache ile Sepet verisi getiriyoruz
                var model = _memoryCache.Get<Sepet>(data);

                if (model != null)
                {
                    sepet.Add(model);
                }
            }



            return Ok(sepet);
        }


        //ekleme
        [HttpPost]
        public IActionResult Add(Sepet sepet)
        {
            //varmı ürün onu kontorl ediyoruz
            var sepettevar = false;
            for (int i = 0; i < 20; i++)
            {

                var data = i.ToString();
                var model = _memoryCache.Get<Sepet>(data);
                if (model != null)
                {
                    if (sepet.Id == model.Id)
                    {
                        sepettevar = true;
                        break;
                    }
                }

            }
            //yoksa ekleme işlemini yapoıyoruz
            List<Sepet> sepet2 = new List<Sepet>();

            for (int i = 0; i < 20; i++)
            {
                var data = i.ToString();
                var model = _memoryCache.Get<Sepet>(data);

                if (model != null)
                {
                    sepet2.Add(model);
                }
            }

            var sayac = sepet2.Count + 1;
            if (!sepettevar)
            {
                _memoryCache.Set<Sepet>(sayac.ToString(), sepet);
                return Ok("ok");
            }

            return NotFound();



        }


        //tümüsilme
        [HttpDelete("All")]
        public IActionResult Delate()
        {
            List<Sepet> sepet2 = new List<Sepet>();

            for (int i = 0; i < 20; i++)
            {

                var data = i.ToString();
                var model = _memoryCache.Get<Sepet>(data);

                if (model != null)
                {
                    sepet2.Add(model);
                }
            }
            for (int i = 0; i < sepet2.Count; i++)
            {
                var data = i + 1;
                _memoryCache.Remove(data.ToString());

            }


            return Ok("ok");
        }



        //tewk ürün silme
        [HttpDelete("{Id}")]
        public IActionResult Delate(string Id)
        {


            List<Sepet> sepet2 = new List<Sepet>();

            for (int i = 0; i < 20; i++)
            {

                var data = i.ToString();
                var model = _memoryCache.Get<Sepet>(data);

                if (model != null)
                {
                    sepet2.Add(model);
                }
            }

            for (int i = 0; i < sepet2.Count; i++)
            {
                if (sepet2[i].Id == Id)
                {
                    var data = i + 1;
                    _memoryCache.Remove(data.ToString());
                }
            }

            return Ok("ok");
        }

    }
}
