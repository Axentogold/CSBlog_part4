using API.Contracts.Users;
using AutoMapper;
using CSBlog.Core.Repository;
using CSBlog.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;


  public UsersController(IMapper mapper, IUnitOfWork unitOfWork)
  {
    _mapper = mapper;
    _unitOfWork = unitOfWork;
  }

  [HttpGet]
  [Route("")]
  public IActionResult Get()
  {
    var users = _unitOfWork.User.GetUsers().ToList();

    var resp = new GetUsersResponse
    {
      UsersTotal = users.Count,
      Users = _mapper.Map<List<BlogUser>, List<UserView>>(users)
    };

    return StatusCode(200, resp);
  }

  [HttpPost]
  [Route("Add")]
  public async Task<IActionResult> Add([FromBody] AddUserRequest request)
  {
    var user = _unitOfWork.User.GetUsers().FirstOrDefault(u => u.UserName == request.UserName);
    if (user != null) return StatusCode(400, $"Error: User '{request.UserName}' already exists.");

    var newUser = _mapper.Map<AddUserRequest, BlogUser>(request);
    await _unitOfWork.User.AddUser(newUser);

    return StatusCode(200, $"User {request.UserName} added successfully. User Id: {newUser.Id}");
  }

  [HttpPatch]
  [Route("{userName}")]
  public IActionResult Edit(
    [FromRoute] string userName,
    [FromBody] EditUserRequest request)
  {
    var user = _unitOfWork.User.GetUserByName(userName);
    if (user == null)
      return StatusCode(400, $"Error: No such user {userName}");

    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.Updated = DateTime.UtcNow;

    _unitOfWork.User.UpdateUser(user);

    return StatusCode(200,
      $"User {userName} is updated successfully. " +
      $"First name: {request.FirstName} " +
      $"Last name: {request.LastName}");
  }

  [HttpDelete]
  [Route("{userName}")]
  public async Task<IActionResult> Delete(
    [FromRoute] string userName
  )
  {
    var user = _unitOfWork.User.GetUserByName(userName);
    if (user == null)
      return StatusCode(400, $"Error: No such user {userName}");
  
    await _unitOfWork.User.DeleteUser(user.Id);
    return StatusCode(200,
      $"User {userName} is successfully deleted");
  }
}