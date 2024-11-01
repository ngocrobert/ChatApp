using API.Entities;
using DatingApp2.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApp2.Data
{
    public class Seed
    {
        public static async Task SeedUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if(users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name="Member"},
                new AppRole{Name="Admin"},
                new AppRole{Name="Moderator"}
            };

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach(var user in users)
            {
                //using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();

                // dung Identity thay the
                //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Ngoc18."));
                //user.PasswordSalt = hmac.Key;

                await userManager.CreateAsync(user, "Ngoc18.");
                await userManager.AddToRoleAsync(user, "Member");

            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Ngoc18.");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

            //await context.SaveChangesAsync();
        }
    }
}
