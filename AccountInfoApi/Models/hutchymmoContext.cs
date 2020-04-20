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

        public virtual DbSet<Characters> Characters { get; set; }
        public virtual DbSet<FriendRequests> FriendRequests { get; set; }
        public virtual DbSet<FriendsList> FriendsList { get; set; }
        public virtual DbSet<UserInformation> UserInformation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=HUTCHYSERVER;Port=3306;Database=hutchymmo;Uid=callumhutchy;Pwd=gwynedd070995;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Characters>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Name });

                entity.ToTable("characters", "hutchymmo");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.InventoryId)
                    .HasColumnName("inventory_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasColumnName("position")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Skin)
                    .HasColumnName("skin")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.WalletId)
                    .HasColumnName("wallet_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zone)
                    .HasColumnName("zone")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FriendRequests>(entity =>
            {
                entity.HasKey(e => new { e.RecipientId, e.SenderId });

                entity.ToTable("friend_requests", "hutchymmo");

                entity.Property(e => e.RecipientId)
                    .HasColumnName("recipient_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SenderId)
                    .HasColumnName("sender_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Sent).HasColumnName("sent");
            });

            modelBuilder.Entity<FriendsList>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.FriendsId });

                entity.ToTable("friends_list", "hutchymmo");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FriendsId)
                    .HasColumnName("friends_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Since).HasColumnName("since");
            });

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.ToTable("user_information", "hutchymmo");

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

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnName("salt")
                    .HasColumnType("blob");

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
                    .IsRequired()
                    .HasColumnName("validation_code")
                    .HasMaxLength(7)
                    .IsUnicode(false);
            });
        }
    }
}
