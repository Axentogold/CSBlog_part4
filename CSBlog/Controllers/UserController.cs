using System.Diagnostics;
using CSBlog.Core;
using CSBlog.Core.Repository;
using CSBlog.Core.ViewModels.User;
using CSBlog.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSBlog.Controllers;

public class UserController : Controller
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly SignInManager<BlogUser> _signInManager;

  public UserController(IUnitOfWork unitOfWork, SignInManager<BlogUser> signInManager)
  {
    _unitOfWork = unitOfWork;
    _signInManager = signInManager;
  }

  // GET
  [Authorize(Policy = Constants.Policies.RequireModerator)]
  public IActionResult Index()
  {
    var users = _unitOfWork.User.GetUsers();
    return View(users);
  }

  [HttpGet]
  [Authorize(Policy = Constants.Policies.RequireAdmin)]
  public async Task<IActionResult> Edit(string id)
  {
    var user = _unitOfWork.User.GetUserById(id);

    var roles = _unitOfWork.Role.GetRoles();

    Debug.Assert(user != null, nameof(user) + " != null");
    var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

    var roleListItems = roles.Select(role =>
      new SelectListItem(
        role.Name,
        role.Id,
        userRoles.Any(ur => ur.Contains(role.Name)))).ToList();


    var vm = new EditUserViewModel
    {
      BlogUser = user,
      UserRoles = roleListItems
    };

    return View(vm);
  }

  [HttpPost]
  [Authorize(Policy = Constants.Policies.RequireAdmin)]
  public async Task<IActionResult> Edit(EditUserViewModel data)
  {
    if (!ModelState.IsValid) return View(data);

    var user = _unitOfWork.User.GetUserById(data.BlogUser.Id);
    if (user == null) return NotFound();

    user.FirstName = data.BlogUser.FirstName;
    user.LastName = data.BlogUser.LastName;
    user.Email = data.BlogUser.Email;
    List<string> rolesToAdd = new();
    List<string> rolesToRemove = new();

    var userRoles = await _signInManager.UserManager.GetRolesAsync(user);
    foreach (var role in data.UserRoles)
    {
      if (role.Selected && !userRoles.Contains(role.Text)) rolesToAdd.Add(role.Text);
      if (!role.Selected && userRoles.Contains(role.Text)) rolesToRemove.Add(role.Text);
    }

    if (rolesToAdd.Any()) await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);

    if (rolesToRemove.Any()) await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
    user.Updated = DateTime.Now;

    _unitOfWork.User.UpdateUser(user);

    return RedirectToAction("Index");
  }

  [HttpGet]
  [Authorize(Policy = Constants.Policies.RequireAdmin)]
  public IActionResult Delete(BlogUser data)
  {
    var user = _unitOfWork.User.GetUserById(data.Id);

    return View(user);
  }

  [HttpPost]
  [Authorize(Policy = Constants.Policies.RequireAdmin)]
  public async Task<IActionResult> Delete(string id)
  {
    await _unitOfWork.User.DeleteUser(id);

    return RedirectToAction("Index");
  }
}