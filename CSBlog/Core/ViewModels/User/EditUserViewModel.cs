using CSBlog.Models.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSBlog.Core.ViewModels.User;

public class EditUserViewModel
{
  public BlogUser BlogUser { get; set; } = null!;

  public IList<SelectListItem> UserRoles { get; set; } = null!;
}