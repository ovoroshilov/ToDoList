using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;

namespace ToDoListDAL
{
    public class AppDbContext : DbContext
    {

        public DbSet<TaskEntity> Tasks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
          //  Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }

}
