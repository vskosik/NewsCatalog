using NewsCatalog.DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatalog.DAL.Repositories
{
    public class BookmarkRepository : GenericRepository<Bookmark>
    {
        public BookmarkRepository(DbContext context) : base(context)
        {
        }
    }
}
