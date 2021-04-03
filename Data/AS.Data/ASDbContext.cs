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
        public DbSet<GenderType> GenderTypes { get; set; }
        public ASDbContext(DbContextOptions<ASDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {              
            base.OnModelCreating(builder);

            builder.Entity<GenderType>().HasData(new GenderType
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Woman"
            });

            builder.Entity<GenderType>().HasData(new GenderType
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Man"
            });

            builder.Entity<GenderType>().HasData(new GenderType
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Kids"
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
