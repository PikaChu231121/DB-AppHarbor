using Microsoft.EntityFrameworkCore;
using AppHarbor.Server.Models;

namespace AppHarbor.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> user { get; set; }
        public DbSet<Merchant> merchant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置表和列名
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "C##APPHARBOR");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255).HasColumnName("PASSWORD");
                entity.Property(e => e.Nickname).IsRequired().HasMaxLength(255).HasColumnName("NICKNAME");
                entity.Property(e => e.Avatar).HasMaxLength(255).HasColumnName("AVATAR");
                entity.Property(e => e.RegisterTime).IsRequired().HasColumnName("REGISTER_TIME");
                entity.Property(e => e.Credit).HasColumnName("CREDIT");
                entity.Property(e => e.State).IsRequired().HasMaxLength(255).HasColumnName("STATE");
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.ToTable("merchant", "C##APPHARBOR");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255).HasColumnName("PASSWORD");
                entity.Property(e => e.Nickname).IsRequired().HasMaxLength(255).HasColumnName("NICKNAME");
                entity.Property(e => e.Avatar).HasMaxLength(255).HasColumnName("AVATAR");
                entity.Property(e => e.RegisterTime).IsRequired().HasColumnName("REGISTER_TIME");
                entity.Property(e => e.Credit).HasColumnName("CREDIT");
                entity.Property(e => e.State).IsRequired().HasMaxLength(255).HasColumnName("STATE");
            });
        }



    }
}
