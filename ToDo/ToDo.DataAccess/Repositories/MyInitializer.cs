using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Common.Helpers;
using ToDo.Entities;

namespace ToDo.DataAccess.Repositories
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user..
            TodoUser admin = new TodoUser()
            {
                Name = "Gökçenur",
                Surname = "Zenginal",
                Email = "zenginalgokcenur@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "gkctnn",
                ProfileImageFilename = "user_boy.png",
                Password = HashHelper.TextSifrele("123456"),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "gkctnn"
            };

            // Adding standart user..
            TodoUser standartUser = new TodoUser()
            {
                Name = "Rana",
                Surname = "Zenginal",
                Email = "ranazenginal@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "ranazenginal",
                Password = HashHelper.TextSifrele("654321"),
                ProfileImageFilename = "user_boy.png",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "gkctnn"
            };

            context.TodoUsers.Add(admin);
            context.TodoUsers.Add(standartUser);
             
            for (int i = 0; i < 8; i++)
            {
                TodoUser user = new TodoUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFilename = "user_boy.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = HashHelper.TextSifrele("123"),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };

                context.TodoUsers.Add(user);
            }

            context.SaveChanges();

            // User list for using..
            List<TodoUser> userlist = context.TodoUsers.ToList();

            // Adding fake categories..
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now.AddYears(-1),
                    ModifiedOn = DateTime.Now.AddYears(-1),
                    ModifiedUsername = "gkctnn"
                };

                context.Categories.Add(cat);

                // Adding fake notes..
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    DateTime rangeDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now);
                    TodoUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsCompleted = false,
                        Owner = owner,
                        CreatedOn = rangeDate,
                        ModifiedOn = rangeDate.AddDays(FakeData.NumberData.GetNumber(1, 10)),
                        CompletedOn = rangeDate.AddDays(FakeData.NumberData.GetNumber(1, 10)),
                        ModifiedUsername = owner.Username,
                    };

                    cat.Notes.Add(note);


                }

            }

            context.SaveChanges();
        }
    }
}
