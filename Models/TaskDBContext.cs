using Microsoft.EntityFrameworkCore;

namespace TaskMgntAPI.Models
{
     public partial class TaskDBContext : DbContext
        {
            public TaskDBContext()
            {
            }

            public TaskDBContext(DbContextOptions<TaskDBContext> options)
                : base(options)
            {
            }

            public virtual DbSet<Task> Tasks { get; set; }

            public virtual DbSet<Project> Projects { get; set; }

            public virtual DbSet<Payment> Payment { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       => optionsBuilder.UseSqlServer("Data Source=iamanu\\sqlexpress;Initial Catalog=TaskMgntDB;Integrated Security=True;TrustServerCertificate=True");

    }
}
