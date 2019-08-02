using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using ToDo.Bussiness;
using ToDo.Entities;

namespace ToDo.WebApp.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");

            if (result == null)
            {
                ICategoryManager _categoryManager = DependencyResolver.Current.GetService<ICategoryManager>();
                result = _categoryManager.List();

                WebCache.Set("category-cache", result, 20, true);
            }

            return result;
        }

        public static void RemoveCategoriesFromCache()
        {
            Remove("category-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
        public static List<Note> GetNotesFromCache()
        {
            var result = WebCache.Get("note-cache");

            INoteManager _noteManager = DependencyResolver.Current.GetService<INoteManager>();

            if (result == null)
            {
                if (CurrentSession.User != null)
                {
                    result = _noteManager.ListQueryable().Where(x => !x.IsCompleted && !x.IsDeleted && x.Owner.Username == CurrentSession.User.Username && x.CompletedOn <= DateTime.Now).ToList();

                    WebCache.Set("note-cache", result, 1, true);
                }

            }

            return result;
        }

        public static void RemoveNotesFromCache()
        {
            Remove("note-cache");
        }


    }
}