using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GaiaProject.Data;

namespace GaiaProject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171215081421_firstdb")]
    partial class firstdb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GaiaDbContext.Models.AccountViewModels.UserFriend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Remark")
                        .HasMaxLength(50);

                    b.Property<string>("UserId")
                        .HasMaxLength(50);

                    b.Property<string>("UserIdTo")
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .HasMaxLength(50);

                    b.Property<string>("UserNameTo")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("UserFriend");
                });

            modelBuilder.Entity("GaiaDbContext.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int?>("groupid");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GaiaDbContext.Models.HomeViewModels.GameFactionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FactionChineseName")
                        .HasMaxLength(20);

                    b.Property<string>("FactionName")
                        .HasMaxLength(20);

                    b.Property<int>("gameinfo_id");

                    b.Property<string>("gameinfo_name")
                        .HasMaxLength(20);

                    b.Property<string>("kjPostion")
                        .HasMaxLength(20);

                    b.Property<string>("numberBuild")
                        .HasMaxLength(20);

                    b.Property<int>("numberFst1");

                    b.Property<int>("numberFst2");

                    b.Property<int>("rank");

                    b.Property<int>("scoreFst1");

                    b.Property<int>("scoreFst2");

                    b.Property<int>("scoreKj");

                    b.Property<int>("scorePw");

                    b.Property<string>("scoreRound")
                        .HasMaxLength(20);

                    b.Property<int>("scoreTotal");

                    b.Property<string>("userid")
                        .HasMaxLength(20);

                    b.Property<string>("username")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("GameFactionModel");
                });

            modelBuilder.Entity("GaiaDbContext.Models.HomeViewModels.GameInfoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ATTList")
                        .HasMaxLength(50);

                    b.Property<string>("FSTList")
                        .HasMaxLength(20);

                    b.Property<int>("GameStatus");

                    b.Property<int>("IsTestGame");

                    b.Property<string>("MapSelction");

                    b.Property<string>("RBTList")
                        .HasMaxLength(50);

                    b.Property<string>("RSTList")
                        .HasMaxLength(50);

                    b.Property<string>("STT3List")
                        .HasMaxLength(30);

                    b.Property<string>("STT6List")
                        .HasMaxLength(50);

                    b.Property<int>("UserCount");

                    b.Property<DateTime?>("endtime");

                    b.Property<string>("loginfo");

                    b.Property<string>("name")
                        .HasMaxLength(20);

                    b.Property<int>("round");

                    b.Property<string>("scoreFaction");

                    b.Property<DateTime>("starttime");

                    b.Property<string>("userlist");

                    b.Property<string>("username");

                    b.Property<int>("version");

                    b.HasKey("Id");

                    b.ToTable("GameInfoModel");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GaiaDbContext.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GaiaDbContext.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GaiaDbContext.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
