using AS.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AS.Data
{
    public class ASDbContext : IdentityDbContext<ASUser, IdentityRole, string>
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ASDbContext(DbContextOptions<ASDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {              
            base.OnModelCreating(builder);

            string ADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            //create user
            var appUser = new ASUser
            {
                Id = ADMIN_ID,
                Email = "admin@admin.admin",
                EmailConfirmed = true,
                UserName = "admin@admin.admin"
            };

            //set user password
            PasswordHasher<ASUser> ph = new PasswordHasher<ASUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "admin1234");

            //seed user
            builder.Entity<ASUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Coat"
            });
            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Puffer"
            });
            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Jacket"
            });
            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Jeans"
            });
            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "T-Shirt"
            });
            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Other"
            });
        }
    }
}
