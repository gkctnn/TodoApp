using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToDo.Bussiness;
using ToDo.Common.Helpers;
using ToDo.Entities;
using ToDo.WebApp.Models;

namespace ToDo.WebApp.Scheduler
{
    public class RemindingScheduler : IJob
    {
        private readonly INoteManager _noteManager;
        public RemindingScheduler(INoteManager noteManager)
        {
            _noteManager = noteManager;
        }
        public void Execute(IJobExecutionContext context)
        {
            //CacheHelper.GetNotesFromCache();
        }
    }
}