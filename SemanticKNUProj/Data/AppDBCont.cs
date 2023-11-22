using Microsoft.EntityFrameworkCore;
using SemanticKNUProj.Model;

namespace SemanticKNUProj.Data
{
    public class AppDBCont : DbContext
    {
        public AppDBCont()
        {

        }

        public AppDBCont(DbContextOptions<AppDBCont> options) : base(options)
        {

        }

        public DbSet<UserModel> userModel { get; set; }
        public DbSet<AlumniModel> alumniModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<AlumniModel>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            base.OnModelCreating(modelBuilder);
        }

    }
}
