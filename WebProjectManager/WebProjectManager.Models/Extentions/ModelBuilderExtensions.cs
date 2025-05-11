using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProjectManager.Models.Entities;

namespace WebProjectManager.Models.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = Guid.NewGuid();


            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = roleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role",
                IsActive = true
            });

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                Lastname = "Tran Thanh",
                Firstname = "Nam",
                EmailConfirmed = true,
                PhoneNumber = "0968354148",
                PasswordHash = hasher.HashPassword(null, "Abc123!@#"),
                SecurityStamp = string.Empty,
                UrlAvatar = "/upload/avatar/admin1.jpg",
                Address = "Đà Nẵng",
                Gender = true,
                DateOfBirth = new DateTime(2000,06,05),
                CreatedOn = DateTime.Now,
                IsVerification = true,
                IsActive = true,
                UpdatedOn = DateTime.Now,
                UserName = "admin123"

            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

        }
    }
}
