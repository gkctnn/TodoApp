

using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using ToDo.Entities;

namespace ToDo.DataAccess.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TodoUser> TodoUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>()
        //        .HasMany(x => x.Notes)
        //        .WithRequired(x => x.Category)
        //        .WillCascadeOnDelete(true);

        //}
    }
}
