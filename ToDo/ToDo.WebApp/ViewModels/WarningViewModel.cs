﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDo.WebApp.ViewModels
{
    public class WarningViewModel : NotifyViewModelBase<string>
    {
        public WarningViewModel()
        {
            Title = "Uyarı!";
        }
    }
}