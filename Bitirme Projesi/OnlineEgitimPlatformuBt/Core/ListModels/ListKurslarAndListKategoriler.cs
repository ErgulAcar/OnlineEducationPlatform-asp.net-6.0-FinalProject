
using Core.Models;
namespace Core.ListModels
{
    public class ListKurslarAndListKategoriler
    {
        public List<Kurs>? kurslar { get; set; }
        public List<Kategori>? kategoriler { get; set; }

        public string? kategori { get; set; }
    }
}
