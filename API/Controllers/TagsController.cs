using API.Contracts.Tags;
using AutoMapper;
using CSBlog.Core.Repository;
using CSBlog.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;

  public TagsController(IMapper mapper, IUnitOfWork unitOfWork)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
  }

  [HttpGet]
  [Route("")]
  public IActionResult Get()
  {
    var tags = _unitOfWork.Tag.GetAll().ToList();

    var resp = new GetTagsResponse
    {
      TagsTotal = tags.Count,
      Tags = _mapper.Map<List<Tag>, List<TagView>>(tags)
    };

    return StatusCode(200, resp);
  }

  [HttpPost]
  [Route("Add")]
  public async Task<IActionResult> Add([FromBody] AddTagRequest request)
  {
    var tag = _unitOfWork.Tag.GetByName(request.TagName);
    if (tag.Id != "0") return StatusCode(400, $"Error: Tag '{request.TagName}' already exists.");

    var newTag = _mapper.Map<AddTagRequest, Tag>(request);
    await _unitOfWork.Tag.Create(newTag);

    return StatusCode(200, $"Tag {request.TagName} added successfully. Tag Id: {newTag.Id}");
  }

  [HttpPatch]
  [Route("{tagName}")]
  public async Task<IActionResult> Edit(
    [FromRoute] string tagName,
    [FromBody] EditTagRequest request)
  {
    var tag = _unitOfWork.Tag.GetByName(tagName);
    if (tag.Id == "0")
      return StatusCode(400, $"Error: No such tag {tagName}");

    tag.TagName = request.NewTagName;

    await _unitOfWork.Tag.Update(tag);

    return StatusCode(200,
      $"Tag {tagName} is renamed to {request.NewTagName}");
  }

  [HttpDelete]
  [Route("{tagName}")]
  public async Task<IActionResult> Delete(
    [FromRoute] string tagName
  )
  {
    var tag = _unitOfWork.Tag.GetByName(tagName);
    if (tag.Id == "0")
      return StatusCode(400, $"Error: No such tag {tagName}");

    await _unitOfWork.Tag.Delete(tag);
    return StatusCode(200,
      $"Tag {tagName} is successfully deleted");
  }
}