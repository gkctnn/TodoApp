using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Core.DataAccess;
using ToDo.Core.Repositories;
using ToDo.Entities;

namespace ToDo.Bussiness
{
    public interface ICategoryManager
    {
        new int Delete(Category category);
        IQueryable<Category> ListQueryable();
        Category Find(Expression<Func<Category, bool>> where);
        int Insert(Category obj);
        int Update(Category obj);
        List<Category> List();
    }
}
