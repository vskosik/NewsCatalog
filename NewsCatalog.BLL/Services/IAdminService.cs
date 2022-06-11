using NewsCatalog.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.Services
{
    public interface IAdminService
    {
        AdminDTO Create(AdminDTO adminDTO);
        void Delete(AdminDTO adminDTO);
        AdminDTO Get(int id);
        IEnumerable<AdminDTO> GetAll();
        Task<IEnumerable<AdminDTO>> GetAllAsync();
        void Update(AdminDTO adminDTO);
    }
}
