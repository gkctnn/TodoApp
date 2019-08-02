using ToDo.Common;
using ToDo.Entities;
using ToDo.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDo.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            TodoUser user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}