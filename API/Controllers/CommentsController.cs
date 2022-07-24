using API.Contracts.Comments;
using AutoMapper;
using CSBlog.Core.Repository;
using CSBlog.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;

  public CommentsController(IMapper mapper, IUnitOfWork unitOfWork)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
  }

  [HttpGet]
  [Route("")]
  public IActionResult Get()
  {
    var comments = _unitOfWork.Comment.GetAll().ToList();


    var resp = new GetCommentsResponse
    {
      CommentsTotal = comments.Count,

      Comments = _mapper.Map<List<Comment>, List<CommentView>>(comments)
    };

    return StatusCode(200, resp);
  }

  [HttpPost]
  [Route("Add/{articleTitle}")]
  public async Task<IActionResult> Add([FromRoute] string articleTitle, [FromBody] AddCommentRequest request)
  {
    var article = _unitOfWork.Article.GetByName(articleTitle);
    if (article.Id == "0") return StatusCode(400, $"Error: Article '{articleTitle}' not found.");

    var newComment = _mapper.Map<AddCommentRequest, Comment>(request);
    var author = _unitOfWork.User.GetUsers().FirstOrDefault(u => u.GetFullName() == request.Author);
    if (author == null) return StatusCode(400, $"Error: No such User as Author: '{request.Author}' ");

    newComment.UserId = author.Id;
    newComment.ArticleId = article.Id;

    await _unitOfWork.Comment.Create(newComment);

    return StatusCode(200, $"Article {articleTitle} received a new comment. Comment text: {request.Text}");
  }


  [HttpPatch]
  [Route("{id}")]
  public async Task<IActionResult> Edit(
    [FromRoute] string id,
    [FromBody] EditCommentRequest request)
  {
    var comment = _unitOfWork.Comment.GetById(id);
    if (comment.Id == "0")
      return StatusCode(400, $"Error: No such comment");

    comment.Text = request.Text;
    comment.Changed = DateTime.UtcNow;

    await _unitOfWork.Comment.Update(comment);

    return StatusCode(200,
      $"Comment {request.Text} is successfully updated.");
  }

  [HttpDelete]
  [Route("{id}")]
  public async Task<IActionResult> Delete(
    [FromRoute] string id
  )
  {
    var comment = _unitOfWork.Comment.GetById(id);
    if (comment.Id == "0")
      return StatusCode(400, $"Error: No such comment");

    await _unitOfWork.Comment.Delete(comment);
    return StatusCode(200,
      $"Comment {comment.Text} is successfully deleted");
  }
}