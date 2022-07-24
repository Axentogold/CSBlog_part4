using CSBlog.Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Controllers;

public class CommentController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
  // private readonly ApplicationDbContext _context;

  public CommentController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  // GET
  public IActionResult GetArticleComments(string articleId)
  {
    var comments = _unitOfWork.Comment.GetAll().Where(t => t.ArticleId == articleId);
    
    return Json(comments);
  }
}