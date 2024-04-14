using simple_authentication_client_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace simple_authentication_client_infrastructure.Context;

public partial class TemplateAppContext : DbContext
{
    public TemplateAppContext(DbContextOptions<TemplateAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("db_owner");


        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "dbo");

            entity.Property(e => e.UserName).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.ToTable("UserSession", "dbo");

            entity.Property(e => e.Token);

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSession_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
