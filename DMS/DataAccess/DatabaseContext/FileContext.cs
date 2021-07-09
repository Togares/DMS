using CommonTypes;
using System.Data.Entity;

namespace DataAccess.DatabaseContext
{
    public class FileContext : ContextBase
    {
        public FileContext(Connection existingConnection, bool ownsConnection)
            : base (existingConnection, ownsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("DMS");
            modelBuilder.Entity<File>().ToTable("file");
            modelBuilder.Entity<File>().HasKey(x => x.ID);
            modelBuilder.Entity<File>().Property(x => x.ID).HasColumnName("id");
            modelBuilder.Entity<File>().Property(x => x.Content).HasColumnName("content");
            modelBuilder.Entity<File>().Property(x => x.Created).HasColumnName("created");
            modelBuilder.Entity<File>().Property(x => x.Modified).HasColumnName("modified");
            modelBuilder.Entity<File>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<File>().Property(x => x.Path).HasColumnName("path");
            modelBuilder.Entity<File>().Property(x => x.Type).HasColumnName("type");
        }

        public virtual DbSet<File> Files { get; set; }
    }
}
