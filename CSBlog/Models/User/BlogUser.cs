using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CSBlog.Models.User;

public class BlogUser : IdentityUser
{
  // public Guid Id { get; set; } = Guid.NewGuid();

  // [Required(ErrorMessage = "This field is required.")]
  // [StringLength(32)]


  public string FirstName { get; set; } = string.Empty;

  // [Required(ErrorMessage = "This field is required.")]
  // [StringLength(32)]
  public string LastName { get; set; } = string.Empty;


  [NotMapped]
  // [FileExtensions(Extensions = "jpg, jpeg, png, svg")]
  public IFormFile? Avatar { get; set; }

  // [Required(ErrorMessage = "This field is required.")]
  // [StringLength(16, MinimumLength = 6)]
  // [PasswordPropertyText]
  // public string Password { get; set; } = string.Empty;

  // public List<UserRole> UserRoleList { get; set; } = null!;

  public DateTime Registered { get; set; } = DateTime.Now;
  public DateTime Updated { get; set; }

  public string GetFullName()
  {
    return $"{FirstName} {LastName}";
  }
}