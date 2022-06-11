using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.DTO
{
    public class BookmarkDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string SourceUrl { get; set; }
        public string ImageUrl { get; set; }
        public string PublishedData { get; set; }
        public string Content { get; set; }
        public bool IsBanned { get; set; }
    }
}
