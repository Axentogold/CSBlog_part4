using System.Security.Claims;
using CSBlog.Core;
using CSBlog.Core.Repository;
using CSBlog.Core.ViewModels.Blog;
using CSBlog.Data;
using CSBlog.Models.Blog;
using CSBlog.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Controllers;

public class ArticleController : Controller
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ApplicationDbContext _context;
  private readonly UserManager<BlogUser> _userManager;

  public ArticleController(IUnitOfWork unitOfWork, ApplicationDbContext applicationDbContext,
    UserManager<BlogUser> userManager)
  {
    _unitOfWork = unitOfWork;
    _context = applicationDbContext;
    _userManager = userManager;
  }


  [AllowAnonymous]
  public IActionResult Index()
  {
    var articles = _unitOfWork.Article.GetAll();
    var articleTags = GetArticleTags();
    var vm = new ArticleTagsViewModel
    {
      Articles = articles,
      Tags = articleTags
    };

    return View(vm);
  }


  [HttpGet]
  [AllowAnonymous]
  public IActionResult Read(string? articleId)
  {
    var article = GetArticleEntity(articleId, out var articleTags);

    var vm = new CommentViewModel
    {
      Article = article
    };

    return View(vm);
  }


  [HttpPost]
  [Authorize(Roles = Constants.Roles.User)]
  public async Task<IActionResult> Read(CommentViewModel data, string? articleId)
  {
    if (!ModelState.IsValid) return Json(data);
    var article = GetArticleEntity(articleId, out var articleTags);

    var comment = new Comment
    {
      ArticleId = articleId,
      Text = data.Text,
      Added = DateTime.Now,
      UserId = _userManager.GetUserId(User),
      Id = Guid.NewGuid().ToString()
    };

    article.Comments?.Add(comment);

    await _unitOfWork.Article.Update(article);
    var vm = new CommentViewModel
    {
      Article = article
    };

    return View(vm);
  }


  [HttpGet]
  [Authorize(Roles = Constants.Roles.User)]
  public IActionResult EditComment(string? commentId)
  {
    var comment = _unitOfWork.Comment.GetById(commentId);

    var articleId = _unitOfWork.Article.GetById(comment.ArticleId).Id;
    var article = GetArticleEntity(articleId, out var articleTags);

    var vm = new CommentViewModel
    {
      Article = article,
      Text = comment.Text,
      CommentId = comment.Id
    };

    return View(vm);
  }


  [HttpPost]
  [Authorize(Roles = Constants.Roles.User)]
  public async Task<IActionResult> EditComment(CommentEditViewModel data, string? commentId)
  {
    if (!ModelState.IsValid) return Json(data);
    var comment = _unitOfWork.Comment.GetById(commentId);
    comment.Text = data.Text;
    await _unitOfWork.Comment.Update(comment);
    return RedirectToAction("Read", "Article", new { articleId = data.ArticleId });
  }


  [Authorize(Roles = Constants.Roles.User)]
  public async Task<IActionResult> DeleteComment(string? id)
  {
    var comment = _unitOfWork.Comment.GetById(id);
    await _unitOfWork.Comment.Delete(comment);

    return RedirectToAction("Read", "Article", new { articleId = comment.ArticleId });
  }


  [HttpGet]
  [Authorize(Roles = Constants.Roles.Author)]
  public IActionResult Create()
  {
    var article = new Article();

    var tagListItems = TagListItems();

    var vm = new ArticleViewModel
    {
      Article = article,
      Tags = tagListItems
    };

    return View(vm);
  }


  [HttpPost]
  [Authorize(Roles = Constants.Roles.Author)]
  public async Task<IActionResult> Create(ArticleViewModel data)
  {
    if (!ModelState.IsValid) return Json(data);

    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var user = _unitOfWork.User.GetUserById(userId!);
    var userName = user?.GetFullName();

    var tagList = TagList(data);


    var article = new Article
    {
      Title = data.Article?.Title,
      Text = data.Article?.Text,

      Comments = data.Article?.Comments,
      UserId = userId!,
      Author = userName!,
      Published = DateTime.Now,
      Id = Guid.NewGuid().ToString(),
      Tags = tagList
    };

    await _unitOfWork.Article.Create(article);

    return RedirectToAction("Index");
  }


  [HttpGet]
  [Authorize(Roles = Constants.Roles.Author)]
  public IActionResult Update(string? id)
  {
    var article = _unitOfWork.Article.GetById(id);
    GetArticleTags();
    var tagListItems = TagListItems();

    foreach (var tag in tagListItems)
      if (article.Tags != null && article.Tags.Any(a => a?.Id == tag.Value))
        tag.Selected = true;

    var articleVm = new ArticleViewModel
    {
      Article = article,
      Tags = tagListItems
    };
    return View(articleVm);
  }

  [HttpPost]
  [Authorize(Roles = Constants.Roles.Author)]
  public async Task<IActionResult> Update(ArticleViewModel data, string? id)
  {
    if (!ModelState.IsValid) return View(data);

    var tagList = TagList(data);
    var article = _unitOfWork.Article.GetById(id);
    GetArticleTags();

    article.Title = data.Article?.Title;
    article.Text = data.Article?.Text;
    article.Tags = tagList;
    article.Edited = DateTime.UtcNow;
    await _unitOfWork.Article.Update(article);

    return RedirectToAction("Index");
  }


  [Authorize(Roles = Constants.Roles.Author)]
  public async Task<IActionResult> Delete(string? id)
  {
    var article = _unitOfWork.Article.GetById(id);
    await _unitOfWork.Article.Delete(article);

    return RedirectToAction("Index");
  }

  private List<SelectListItem> TagListItems()
  {
    var tags = _unitOfWork.Tag.GetAll();

    var tagListItems = tags.Select(tag =>
    {
      var item = new SelectListItem(
        tag.TagName,
        tag.Id,
        tags.Any(t => t.TagName != null && t.TagName.Equals(t.TagName)))
      {
        Selected = false
      };
      return item;
    }).ToList();

    return tagListItems;
  }

  private ICollection<Tag?> TagList(ArticleViewModel data)
  {
    ICollection<Tag?> tagList = new List<Tag?>();

    if (data.Tags == null) return new List<Tag?>();
    var allTags = _unitOfWork.Tag.GetAll();

    foreach (var tag in data.Tags)
      if (tag.Selected && allTags.FirstOrDefault(t => t.Id == tag.Value) != null)
        tagList.Add(SelectedTag(allTags, tag));

    return tagList;
  }

  private static Tag? SelectedTag(ICollection<Tag> allTags, SelectListItem tag)
  {
    var selectedTag = allTags.FirstOrDefault(t => t.Id == tag.Value);

    return selectedTag;
  }


  private List<ICollection<Tag>?> GetArticleTags()
  {
    var articleTags =
      _context.Articles
        .Include(a => a.Tags).ToList()
        .Select(a => a.Tags).ToList();
    return articleTags!;
  }


  private Article GetArticleEntity(string? articleId, out List<ICollection<Tag>?> articleTags)
  {
    var article = _unitOfWork.Article.GetById(articleId);
    var comments = _unitOfWork.Comment.GetAll().Where(t => t.ArticleId == articleId).ToList();
    article.Comments = comments;
    articleTags = GetArticleTags();
    return article;
  }
}