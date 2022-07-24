using API.Contracts;
using CSBlog.Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork;


  public BlogController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  [HttpGet] 
  [Route("info")]
  public IActionResult Info()
  {
    var articles = _unitOfWork.Article.GetAll().Count;
    var users = _unitOfWork.User.GetUsers().Count;
    var comments = _unitOfWork.Comment.GetAll().Count;
    var tags = _unitOfWork.Tag.GetAll().Count;

    var resp = new BlogInfoResponse
    {
      Articles = articles,
      Users = users,
      Comments = comments,
      Categories = tags
    };
    return StatusCode(200, resp);
  }
}