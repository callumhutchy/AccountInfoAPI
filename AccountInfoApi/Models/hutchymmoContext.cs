using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccountInfoApi.Models
{
    public partial class hutchymmoContext : DbContext
    {
        public hutchymmoContext()
        {
        }

        public hutchymmoContext(DbContextOptions<hutchymmoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserInformation> UserInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.ToTable("user_information", "hutchymmo");

                entity.HasIndex(e => e.Email)
                    .HasName("email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("username_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Admin)
                    .HasColumnName("admin")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AuthenticationToken)
                    .HasColumnName("authentication_token")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfCreation)
                    .HasColumnName("date_of_creation")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DateOfLastLogin).HasColumnName("date_of_last_login");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.LoggedIn)
                    .HasColumnName("logged_in")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PreviousPassword)
                    .HasColumnName("previous_password")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Validated)
                    .HasColumnName("validated")
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ValidationCode)
                    .HasColumnName("validation_code")
                    .HasMaxLength(7)
                    .IsUnicode(false);
            });
        }
    }
}
