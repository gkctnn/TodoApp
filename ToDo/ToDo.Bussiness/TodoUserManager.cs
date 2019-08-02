
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Bussiness.Results;
using ToDo.Common.Helpers;
using ToDo.Core.DataAccess;
using ToDo.Core.Repositories;
using ToDo.Entities;
using ToDo.Entities.Messages;
using ToDo.Entities.ValueObjects;

namespace ToDo.Bussiness
{
    public class TodoUserManager : ITodoUserManager
    {
        private ToDoRepository<Note> _noteRepository;
        private ToDoRepository<TodoUser> _userRepository;

        public TodoUserManager(ToDoRepository<TodoUser> userRepository, ToDoRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public BusinessLayerResult<TodoUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();
            res.Result = _userRepository.Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }

        public int Delete(TodoUser obj)
        {
            foreach (Note note in obj.Notes.ToList())
            {
                _noteRepository.Delete(note);
            }

            return _userRepository.Delete(obj);
        }

        public TodoUser Find(Expression<Func<TodoUser, bool>> where)
        {
            return _userRepository.Find(where);
        }

        public BusinessLayerResult<TodoUser> GetUserById(int id)
        {
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();
            res.Result = _userRepository.Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<TodoUser> Insert(TodoUser data)
        {
            TodoUser user = _userRepository.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user_boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (_userRepository.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public List<TodoUser> List()
        {
            return _userRepository.List();
        }

        public BusinessLayerResult<TodoUser> LoginUser(LoginViewModel data)
        {
            string sifre = HashHelper.TextSifrele(data.Password);
            // Giriş kontrolü
            // Hesap aktive edilmiş mi?
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == sifre);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
            }

            return res;
        }

        public BusinessLayerResult<TodoUser> RegisterUser(RegisterViewModel data)
        {
            // Kullanıcı username kontrolü..
            // Kullanıcı e-posta kontrolü..
            // Kayıt işlemi..
            // Aktivasyon e-postası gönderimi.
            TodoUser user = _userRepository.Find(x => x.Username == data.Username || x.Email == data.EMail);
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                int dbResult = _userRepository.Insert(new TodoUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFilename = "user_boy.png",
                    Password = HashHelper.TextSifrele(data.Password),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (dbResult > 0)
                {
                    res.Result = _userRepository.Find(x => x.Email == data.EMail && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, res.Result.Email, "ToDo Hesap Aktifleştirme");
                }
            }

            return res;
        }

        public BusinessLayerResult<TodoUser> RemoveUserById(int id)
        {
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();
            TodoUser user = _userRepository.Find(x => x.Id == id);

            if (user != null)
            {
                if (_userRepository.Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }



        public BusinessLayerResult<TodoUser> Update(TodoUser data)
        {
            TodoUser db_user = _userRepository.Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = _userRepository.Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = HashHelper.TextSifrele(data.Password);
            //res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (_userRepository.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<TodoUser> UpdateProfile(TodoUser data)
        {
            TodoUser db_user = _userRepository.Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<TodoUser> res = new BusinessLayerResult<TodoUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = _userRepository.Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = HashHelper.TextSifrele(data.Password);
            //res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (_userRepository.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }


    }
}
