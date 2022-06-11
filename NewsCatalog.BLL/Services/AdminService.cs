using AutoMapper;
using NewsCatalog.BLL.DTO;
using NewsCatalog.DAL.Context;
using NewsCatalog.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.Services
{
    public class AdminService : IAdminService
    {
        private AdminRepository adminRepository;
        private IMapper mapper;

        public AdminService(AdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Admin, AdminDTO>()
                    .ForMember(x => x.Id, x => x.MapFrom(admin => admin.AdminId))
                    .ForMember(x => x.Username, x => x.MapFrom(admin => admin.Username))
                    .ForMember(x => x.PasswordHash, x => x.MapFrom(admin => admin.PasswordHash))
                    .ReverseMap();
            });

            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<AdminDTO>> GetAllAsync() => await Task.Run(() => GetAll());

        public IEnumerable<AdminDTO> GetAll() => mapper.Map<IEnumerable<AdminDTO>>(adminRepository.GetAll());

        public AdminDTO Get(int id) => mapper.Map<AdminDTO>(adminRepository.Get(id));

        public void Delete(AdminDTO adminDTO)
        {
            var adminToDelete = adminRepository.Get(adminDTO.Id);
            adminRepository.Delete(adminToDelete);
            adminRepository.Save();
        }

        public void Update(AdminDTO adminDTO)
        {
            var adminToEdit = adminRepository.Get(adminDTO.Id);
            adminDTO.Id = adminToEdit.AdminId;

            adminToEdit = mapper.Map<Admin>(adminDTO);
            adminRepository.CreateOrUpdate(adminToEdit);
            adminRepository.Save();
        }

        public AdminDTO Create(AdminDTO adminDTO)
        {
            var admin = mapper.Map<Admin>(adminDTO);
            adminRepository.CreateOrUpdate(admin);
            adminRepository.Save();
            adminDTO.Id = admin.AdminId;
            return adminDTO;
        }
    }
}
