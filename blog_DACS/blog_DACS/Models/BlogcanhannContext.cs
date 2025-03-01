using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace blog_DACS.Models;

public partial class BlogcanhannContext : DbContext
{
    public BlogcanhannContext()
    {
    }

    public BlogcanhannContext(DbContextOptions<BlogcanhannContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<FavoritePost> FavoritePosts { get; set; }

    public virtual DbSet<FavoriteUser> FavoriteUsers { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Liked> Likeds { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<RoleUser> RoleUsers { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-7R00JHKC;Initial Catalog=BLOGCANHANN;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdPost }).HasName("PK__Comment__F60C34A10A92C3DF");

            entity.ToTable("Comment");

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdPost).HasColumnName("ID_Post");
            entity.Property(e => e.ContentComment)
                .HasMaxLength(200)
                .HasColumnName("Content_Comment");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.ParentComment).HasColumnName("Parent_Comment");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Post_ID");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_User_ID");
        });

        modelBuilder.Entity<FavoritePost>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdPost }).HasName("PK__Favorite__F60C34A13C3C2CD1");

            entity.ToTable("Favorite_Post");

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdPost).HasColumnName("ID_Post");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.FavoritePosts)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_Post_Post_ID");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FavoritePosts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_Post_User_ID");
        });

        modelBuilder.Entity<FavoriteUser>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdFavoriteUser }).HasName("PK__Favorite__E133B7A8FABD13D0");

            entity.ToTable("Favorite_User");

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdFavoriteUser).HasColumnName("ID_Favorite_User");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");

            entity.HasOne(d => d.IdFavoriteUserNavigation).WithMany(p => p.FavoriteUserIdFavoriteUserNavigations)
                .HasForeignKey(d => d.IdFavoriteUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_User_Favorite_ID");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FavoriteUserIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_User_User_ID");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => new { e.IdFollower, e.IdFollowing }).HasName("PK__Follow__A54C4162780E0ED3");

            entity.ToTable("Follow");

            entity.Property(e => e.IdFollower).HasColumnName("ID_Follower");
            entity.Property(e => e.IdFollowing).HasColumnName("ID_Following");
            entity.Property(e => e.ExistenceStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Existence_Status");

            entity.HasOne(d => d.IdFollowerNavigation).WithMany(p => p.FollowIdFollowerNavigations)
                .HasForeignKey(d => d.IdFollower)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Follower_ID");

            entity.HasOne(d => d.IdFollowingNavigation).WithMany(p => p.FollowIdFollowingNavigations)
                .HasForeignKey(d => d.IdFollowing)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Following_ID");
        });

        modelBuilder.Entity<Liked>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdPost }).HasName("PK__Liked__F60C34A102C6EE60");

            entity.ToTable("Liked");

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdPost).HasColumnName("ID_Post");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Likeds)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Thich_Post_ID");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Likeds)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Thich_User_ID");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.IdPermission).HasName("PK__Permissi__D832E15CB1D2BBA0");

            entity.ToTable("Permission");

            entity.Property(e => e.IdPermission)
                .ValueGeneratedNever()
                .HasColumnName("ID_Permission");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DescriptionPermission)
                .HasMaxLength(500)
                .HasColumnName("Description_Permission");
            entity.Property(e => e.NamePermission)
                .HasMaxLength(100)
                .HasColumnName("Name_Permission");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PK__Post__B41D0E30A485DD80");

            entity.ToTable("Post");

            entity.HasIndex(e => e.IdPost, "UQ__Post__B41D0E317F6B06DA").IsUnique();

            entity.Property(e => e.IdPost)
                .ValueGeneratedNever()
                .HasColumnName("ID_Post");
            entity.Property(e => e.ContentPost)
                .HasMaxLength(1000)
                .HasColumnName("Content_Post");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.ExistenceStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Existence_Status");
            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.ImagePost)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Image_Post");
            entity.Property(e => e.LastAccessedAt)
                .HasColumnType("datetime")
                .HasColumnName("Last_Accessed_At");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_User_ID");
        });

        modelBuilder.Entity<RoleUser>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role_Use__43DCD32D24310033");

            entity.ToTable("Role_User");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("ID_Role");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DescriptionRole)
                .HasMaxLength(500)
                .HasColumnName("Description_Role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("Role_Name");

            entity.HasMany(d => d.IdPermissions).WithMany(p => p.IdRoles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("IdPermission")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Role_Permission_Permission_ID"),
                    l => l.HasOne<RoleUser>().WithMany()
                        .HasForeignKey("IdRole")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Role_Permission_Role_ID"),
                    j =>
                    {
                        j.HasKey("IdRole", "IdPermission").HasName("PK__Role_Per__9E5FFD383F15CA88");
                        j.ToTable("Role_Permission");
                        j.IndexerProperty<long>("IdRole").HasColumnName("ID_Role");
                        j.IndexerProperty<long>("IdPermission").HasColumnName("ID_Permission");
                    });
        });

        modelBuilder.Entity<Share>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdPost }).HasName("PK__Share__F60C34A1BC41E6B1");

            entity.ToTable("Share");

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IdPost).HasColumnName("ID_Post");
            entity.Property(e => e.SharedAt)
                .HasColumnType("datetime")
                .HasColumnName("Shared_At");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Shares)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Share_Post_ID");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Shares)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Share_User_ID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__ED4DE44262E4F214");

            entity.HasIndex(e => e.IdUser, "UQ__Users__ED4DE443FA09E27B").IsUnique();

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("ID_User");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("Date_Of_Birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExistenceStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Existence_Status");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.LastUpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Last_Updated_At");
            entity.Property(e => e.Pass)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.PlaceOfBirth)
                .HasMaxLength(150)
                .HasColumnName("Place_Of_Birth");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
