using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ToDo.Bussiness;
using ToDo.Entities;
using ToDo.WebApp.SignalHub;

namespace ToDo.WebApp.Models
{
    public class DbOperations
    {
        private readonly INoteManager _noteManager;
        public DbOperations(INoteManager noteManager)
        {
            _noteManager = noteManager;
        }
        public IEnumerable<Note> NoteGetir()
        {
            var list = _noteManager.ListQueryable().Where(x => x.IsCompleted == false && x.Owner.Username == CurrentSession.User.Username && x.CompletedOn <= DateTime.Now).OrderBy(x => x.ModifiedOn).ToList();
            return list;
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            NoteHub.Show();
        }
    }
}