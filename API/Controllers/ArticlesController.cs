using API.Contracts.Articles;
using API.Contracts.Tags;
using AutoMapper;
using CSBlog.Core.Repository;
using CSBlog.Data;
using CSBlog.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;
  private readonly ApplicationDbContext _context;

  public ArticlesController(IMapper mapper, IUnitOfWork unitOfWork, ApplicationDbContext context)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
    _context = context;
  }

  [HttpGet]
  [Route("")]
  public IActionResult Get()
  {
    var articles = _unitOfWork.Article.GetAll().ToList();

    foreach (var article in articles) GetArticleTags();

    var resp = new GetArticlesResponse
    {
      ArticlesTotal = articles.Count,

      Articles = _mapper.Map<List<Article>, List<ArticleView>>(articles)
    };

    return StatusCode(200, resp);
  }

  [HttpPost]
  [Route("Add")]
  public async Task<IActionResult> Add([FromBody] AddArticleRequest request)
  {
    var article = _unitOfWork.Article.GetByName(request.Title);
    if (article.Id != "0") return StatusCode(400, $"Error: Article '{request.Title}' already exists.");

    var tags = Collection(request.Tags);

    var newArt = _mapper.Map<AddArticleRequest, Article>(request);
    newArt.Tags = tags;
    await _unitOfWork.Article.Create(newArt);

    return StatusCode(200, $"Article {request.Title} added successfully. Article Id: {newArt.Id}");
  }


  [HttpPatch]
  [Route("{title}")]
  public async Task<IActionResult> Edit(
    [FromRoute] string title,
    [FromBody] EditArticleRequest request)
  {
    var article = _unitOfWork.Article.GetByName(title);
    if (article.Id == "0")
      return StatusCode(400, $"Error: No such article {title}");
    GetArticleTags();
    var tags = Collection(request.Tags);

    foreach (var tag in tags)
      if (!article.Tags.Contains(tag))
        article.Tags.Add(tag);

    await _unitOfWork.Article.Update(article);

    return StatusCode(200,
      $"Article {title} is successfully updated. Title: {request.Title}, " +
      $"Text: {request.Text}, Tags: {article.Tags.Count}");
  }

  [HttpDelete]
  [Route("{title}")]
  public async Task<IActionResult> Delete(
    [FromRoute] string title
  )
  {
    var article = _unitOfWork.Article.GetByName(title);
    if (article.Id == "0")
      return StatusCode(400, $"Error: No such article {title}");

    await _unitOfWork.Article.Delete(article);
    return StatusCode(200,
      $"Article {title} is successfully deleted");
  }

  private ICollection<Tag?> Collection(ICollection<TagView> data)
  {
    var allTags = _unitOfWork.Tag.GetAll().ToList();
    ICollection<Tag?> tags = new List<Tag>()!;

    foreach (var tag in data)
    {
      var newTag = allTags.FirstOrDefault(t => t.TagName == tag.TagName);
      if (newTag != null)
        tags.Add(newTag);
    }

    return tags;
  }

  private List<ICollection<Tag>?> GetArticleTags()
  {
    var articleTags =
      _context.Articles
        .Include(a => a.Tags).ToList()
        .Select(a => a.Tags).ToList();
    return articleTags!;
  }
}