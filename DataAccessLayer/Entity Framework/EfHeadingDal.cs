using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity; // Include için gerekli!

namespace DataAccessLayer.Entity_Framework
{
    public class EfHeadingDal : GenericRepository<Heading>, IHeadingDal
    {
        public List<Heading> GetListWithCategory()
        {
            using (var context = new Context())
            {
                return context.Headings.Include(x => x.Category).ToList();
            }
        }
    }
}
