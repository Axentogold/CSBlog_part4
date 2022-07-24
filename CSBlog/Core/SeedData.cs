using CSBlog.Models.User;
using Microsoft.AspNetCore.Identity;

namespace CSBlog.Core;

public static class SeedData
{
  public static void Seed(RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
  {
    SeedRoles(roleManager);
    SeedUsers(userManager);
  }

  private static void SeedUsers(UserManager<BlogUser> userManager)
  {
    if (userManager.Users.FirstOrDefault(bu => bu.Email == "admin@example.com") != null)
    {
      var user = new BlogUser
      {
        FirstName = "Site",
        LastName = "Admin",
        UserName = "admin@example.com",
        Email = "admin@example.com"
      };
      userManager.CreateAsync(user, "admin123!").Wait();
      userManager.AddToRolesAsync(user, new List<string>
      {
        Constants.Roles.Administrator,
        Constants.Roles.Moderator,
        Constants.Roles.Author,
        Constants.Roles.User
      });
      var result = userManager.CreateAsync(user).Result;
    }

    if (userManager.Users.FirstOrDefault(bu => bu.Email == "moder@example.com") != null)
    {
      var user = new BlogUser
      {
        FirstName = "Blog",
        LastName = "Moderator",
        UserName = "moder@example.com",
        Email = "moder@example.com"
      };
      userManager.CreateAsync(user, "moder123!").Wait();
      userManager.AddToRolesAsync(user, new List<string>
      {
        Constants.Roles.Moderator,
        Constants.Roles.Author,
        Constants.Roles.User
      });
      var result = userManager.CreateAsync(user).Result;
    }

    if (userManager.Users.FirstOrDefault(bu => bu.Email == "author@example.com") != null)
    {
      var user = new BlogUser
      {
        FirstName = "Fancy",
        LastName = "Author",
        UserName = "author@example.com",
        Email = "author@example.com"
      };
      userManager.CreateAsync(user, "author123!").Wait();
      userManager.AddToRolesAsync(user, new List<string>
      {
        Constants.Roles.Author,
        Constants.Roles.User
      });
      var result = userManager.CreateAsync(user).Result;
    }

    if (userManager.Users.FirstOrDefault(bu => bu.Email == "user@example.com") != null)
    {
      var user = new BlogUser
      {
        FirstName = "Regular",
        LastName = "User",
        UserName = "user@example.com",
        Email = "user@example.com"
      };
      userManager.CreateAsync(user, "user123!").Wait();
      userManager.AddToRolesAsync(user, new List<string>
      {
        Constants.Roles.User
      });
      var result = userManager.CreateAsync(user).Result;
    }
  }

  private static void SeedRoles(RoleManager<IdentityRole> roleManager)
  {
    if (!roleManager.RoleExistsAsync(Constants.Roles.Administrator).Result)
    {
      var role = new IdentityRole
      {
        Name = Constants.Roles.Administrator
      };
      var result = roleManager.CreateAsync(role).Result;
    }

    if (!roleManager.RoleExistsAsync(Constants.Roles.Moderator).Result)
    {
      var role = new IdentityRole
      {
        Name = Constants.Roles.Moderator
      };
      var result = roleManager.CreateAsync(role).Result;
    }

    if (!roleManager.RoleExistsAsync(Constants.Roles.Author).Result)
    {
      var role = new IdentityRole
      {
        Name = Constants.Roles.Author
      };
      var result = roleManager.CreateAsync(role).Result;
    }

    if (!roleManager.RoleExistsAsync(Constants.Roles.User).Result)
    {
      var role = new IdentityRole
      {
        Name = Constants.Roles.User
      };
      var result = roleManager.CreateAsync(role).Result;
    }
  }
}