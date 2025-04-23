using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace CESI_ToDoListWPF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todolist.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>().HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
        }

        public void InitializeDatabase()
        {
            using var context = new ApplicationDbContext();
            context.Database.EnsureCreated();
        }

        public ObservableCollection<ToDoTask> LoadTasks()
        {
            using var context = new ApplicationDbContext();
            var tasks = context.ToDoTasks
            .OrderBy(t => t.DateTime)
            .ToList();
            return new ObservableCollection<ToDoTask>(tasks);
        }

        public void SaveTasks(ObservableCollection<ToDoTask> tasks)
        {
            using var context = new ApplicationDbContext();
            context.ToDoTasks.RemoveRange(context.ToDoTasks);
            context.ToDoTasks.AddRange(tasks);
            context.SaveChanges();
        }

        public void UpdateTask(ToDoTask task)
        {
            using var context = new ApplicationDbContext();
            context.ToDoTasks.Update(task);
            context.SaveChanges();
        }
    }
}
