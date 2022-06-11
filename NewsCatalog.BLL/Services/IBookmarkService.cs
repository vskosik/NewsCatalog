using NewsCatalog.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.Services
{
    public interface IBookmarkService
    {
        BookmarkDTO Create(BookmarkDTO articleDTO);
        void Delete(BookmarkDTO articleDTO);
        BookmarkDTO Get(int id);
        IEnumerable<BookmarkDTO> GetAll();
        Task<IEnumerable<BookmarkDTO>> GetAllAsync();
        void Update(BookmarkDTO articleDTO);
    }
}
