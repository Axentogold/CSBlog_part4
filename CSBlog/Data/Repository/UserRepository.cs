using CSBlog.Core.Repository;
using CSBlog.Models.User;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Data.Repository;

public class UserRepository : IUserRepository
{
  private readonly ApplicationDbContext _context;

  public UserRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public ICollection<BlogUser> GetUsers()
  {
    return _context.BlogUsers.ToList();
  }

  public async Task AddUser(BlogUser user)
  {
    await _context.AddAsync(user);
    await _context.SaveChangesAsync();
  }

  public async Task EditUser(BlogUser user)
  {
    var blogUser = await _context.BlogUsers.FindAsync(user.Id);

    if (blogUser != null)

    {
      if (user.FirstName != string.Empty) blogUser.FirstName = user.FirstName;
      if (user.LastName != string.Empty) blogUser.LastName = user.LastName;
      if (user.Avatar != null) blogUser.Avatar = user.Avatar;
    }

    await _context.SaveChangesAsync();
  }

  public BlogUser UpdateUser(BlogUser user)
  {
    _context.Update(user);
    _context.SaveChanges();
    return user;
  }

  public BlogUser? GetUserById(string userId)
  {
    var user = _context.BlogUsers.Find(userId);
    // Console.WriteLine("User by Id: " + user?.Login);
    return user ?? null;
  }

  public BlogUser? GetUserByName(string userName)
  {
    return _context.BlogUsers.FirstOrDefault(u => u.UserName == userName);
  }

  public async Task<BlogUser[]> GetAllUsers()
  {
    return await _context.BlogUsers.ToArrayAsync();
  }

  public async Task DeleteUser(string userId)
  {
    var user = GetUserById(userId);
    if (user != null) _context.BlogUsers.Remove(user);

    await _context.SaveChangesAsync();
  }
}