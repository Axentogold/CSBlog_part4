using CSBlog.Core.Repository;
using CSBlog.Core.ViewModels.Blog;
using CSBlog.Data;
using CSBlog.Models.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Constants = CSBlog.Core.Constants;

namespace CSBlog.Controllers;

[Authorize(Policy = Constants.Policies.RequireModerator)]
public class TagController : Controller
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ApplicationDbContext _context;

  public TagController(IUnitOfWork unitOfWork, ApplicationDbContext context)
  {
    _unitOfWork = unitOfWork;
    _context = context;
  }


  public IActionResult Index()
  {
    var tags = _unitOfWork.Tag.GetAll();
    return View(tags);
  }

  [HttpGet]
  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create(TagViewModel data)
  {
    if (!ModelState.IsValid) return Json(data);

    var tag = new Tag
    {
      TagName = data.TagName,
      Id = Guid.NewGuid().ToString()
    };

    await _unitOfWork.Tag.Create(tag);
    return RedirectToAction("Index");
  }

  [HttpGet]
  public IActionResult Update(string? id)
  {
    var tag = _unitOfWork.Tag.GetById(id);
    var tagVm = new TagViewModel
    {
      TagName = tag.TagName
    };
    return View(tagVm);
  }

  [HttpPost]
  public async Task<IActionResult> Update(TagViewModel data, string? id)
  {
    if (!ModelState.IsValid) return View(data);

    var tag = _unitOfWork.Tag.GetById(id);
    tag.TagName = data.TagName;
    await _unitOfWork.Tag.Update(tag);
    return RedirectToAction("Index");
  }


  public async Task<IActionResult> Delete(string? id)
  {
    var tag = _unitOfWork.Tag.GetById(id);
    await _unitOfWork.Tag.Delete(tag);
    return RedirectToAction("Index");
  }
  
  [HttpGet]
  [AllowAnonymous]
  public IActionResult AllArticlesForTag(string? tagName)
  {
    var tag = _unitOfWork.Tag.GetAll().FirstOrDefault(t => t.TagName == tagName);
    
    var tagArticles =
      _context.Tags
        .Include(a => a.Articles).ToList()
        .Select(a => a.Articles).ToList();
    
    var tagVm = new TagArticleViewModel
    {
      TagName = tagName,
      TagId = tag?.Id,
      Tag = tag
    };
    return View(tagVm);
  }
}