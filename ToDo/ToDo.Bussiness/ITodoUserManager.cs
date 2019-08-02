using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Bussiness.Results;
using ToDo.Core.DataAccess;
using ToDo.Entities;
using ToDo.Entities.ValueObjects;

namespace ToDo.Bussiness
{
    public interface ITodoUserManager
    {
        BusinessLayerResult<TodoUser> RegisterUser(RegisterViewModel data);
        BusinessLayerResult<TodoUser> GetUserById(int id);
        BusinessLayerResult<TodoUser> LoginUser(LoginViewModel data);
        BusinessLayerResult<TodoUser> UpdateProfile(TodoUser data);
        BusinessLayerResult<TodoUser> RemoveUserById(int id);
        BusinessLayerResult<TodoUser> ActivateUser(Guid activateId);
        new BusinessLayerResult<TodoUser> Insert(TodoUser data);
        new BusinessLayerResult<TodoUser> Update(TodoUser data);
        List<TodoUser> List();
        TodoUser Find(Expression<Func<TodoUser, bool>> where);
        int Delete(TodoUser obj);
    }
}
