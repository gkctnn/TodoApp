using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Core.Repositories;
using ToDo.Entities;

namespace ToDo.Bussiness
{
    public class CategoryManager : ICategoryManager
    {
        private ToDoRepository<Note> _noteRepository;
        private ToDoRepository<Category> _categoryRepository;

        public CategoryManager(ToDoRepository<Note> noteRepository, ToDoRepository<Category> categoryRepository)
        {
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
        }

        public int Delete(Category obj)
        {
            // Kategori ile ilişkili notların silinmesi gerekiyor.
            foreach (Note note in obj.Notes.ToList())
            {
                note.IsDeleted = true;
                _noteRepository.Update(note);
            }

            obj.IsDeleted = true;

            return _categoryRepository.Update(obj);
        }

        public Category Find(Expression<Func<Category, bool>> where)
        {
            return _categoryRepository.Find(where);
        }

        public int Insert(Category obj)
        {
            return _categoryRepository.Insert(obj);
        }

        public List<Category> List()
        {
            return _categoryRepository.List();
        }

        public IQueryable<Category> ListQueryable()
        {
            return _categoryRepository.ListQueryable();
        }

        public int Update(Category obj)
        {
            return _categoryRepository.Update(obj);
        }
    }
}
