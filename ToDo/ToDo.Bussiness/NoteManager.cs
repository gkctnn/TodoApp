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
    public class NoteManager : INoteManager
    {
        private ToDoRepository<Note> _noteRepository;

        public NoteManager(ToDoRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public int Delete(Note obj)
        {
            obj.IsDeleted = true;
            return _noteRepository.Update(obj);
        }

        public Note Find(Expression<Func<Note, bool>> where)
        {
            return _noteRepository.Find(where);
        }

        public int Insert(Note obj)
        {
            return _noteRepository.Insert(obj);
        }

        public IQueryable<Note> ListQueryable()
        {
            return _noteRepository.ListQueryable();
        }

        public int Update(Note obj)
        {
            return _noteRepository.Update(obj);
        }
    }
}
