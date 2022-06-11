using AutoMapper;
using NewsCatalog.BLL.DTO;
using NewsCatalog.DAL.Context;
using NewsCatalog.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.BLL.Services
{
    public class BookmarkService : IBookmarkService
    {
        private BookmarkRepository articleRepository;
        private IMapper mapper;

        public BookmarkService(BookmarkRepository articleRepository)
        {
            this.articleRepository = articleRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bookmark, BookmarkDTO>()
                    .ForMember(x => x.Id, x => x.MapFrom(article => article.BookmarkId))
                    .ForMember(x => x.Title, x => x.MapFrom(article => article.Title))
                    .ForMember(x => x.Author, x => x.MapFrom(article => article.Author))
                    .ForMember(x => x.Description, x => x.MapFrom(article => article.Description))
                    .ForMember(x => x.SourceUrl, x => x.MapFrom(article => article.SourceUrl))
                    .ForMember(x => x.ImageUrl, x => x.MapFrom(article => article.ImageUrl))
                    .ForMember(x => x.PublishedData, x => x.MapFrom(article => article.PublishedData))
                    .ForMember(x => x.Content, x => x.MapFrom(article => article.Content))
                    .ForMember(x => x.IsBanned, x => x.MapFrom(article => article.IsBanned))
                    .ReverseMap();
            });

            mapper = new Mapper(configuration);
        }
        public async Task<IEnumerable<BookmarkDTO>> GetAllAsync() => await Task.Run(() => GetAll());

        public IEnumerable<BookmarkDTO> GetAll() => mapper.Map<IEnumerable<BookmarkDTO>>(articleRepository.GetAll());

        public BookmarkDTO Get(int id) => mapper.Map<BookmarkDTO>(articleRepository.Get(id));

        public void Delete(BookmarkDTO articleDTO)
        {
            var articleToDelete = articleRepository.Get(articleDTO.Id);
            articleRepository.Delete(articleToDelete);
            articleRepository.Save();
        }

        public void Update(BookmarkDTO articleDTO)
        {
            var articleToEdit = articleRepository.Get(articleDTO.Id);
            articleDTO.Id = articleToEdit.BookmarkId;

            articleToEdit = mapper.Map<Bookmark>(articleDTO);
            articleRepository.CreateOrUpdate(articleToEdit);
            articleRepository.Save();
        }

        public BookmarkDTO Create(BookmarkDTO articleDTO)
        {
            var article = mapper.Map<Bookmark>(articleDTO);
            articleRepository.CreateOrUpdate(article);
            articleRepository.Save();
            articleDTO.Id = article.BookmarkId;
            return articleDTO;
        }
    }
}
