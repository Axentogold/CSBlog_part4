using CSBlog.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Controllers;

public class RoleController : Controller
{

  [Authorize]
  public IActionResult Index()
  {
    return View();
  }
  
  [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Moderator}")]
  public IActionResult Moderator()
  {
    return View();
  }


  [Authorize(Policy = Constants.Policies.RequireAdmin)]
  public IActionResult Admin()
  {
    return View();
  }
}