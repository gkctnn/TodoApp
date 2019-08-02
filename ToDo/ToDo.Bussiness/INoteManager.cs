using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Core.DataAccess;
using ToDo.Entities;

namespace ToDo.Bussiness
{
    public interface INoteManager
    {
        IQueryable<Note> ListQueryable();
        Note Find(Expression<Func<Note, bool>> where);
        int Insert(Note obj);
        int Update(Note obj);
        int Delete(Note obj);
    }
}
