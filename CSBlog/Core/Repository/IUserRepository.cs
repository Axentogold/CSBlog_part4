using CSBlog.Models.User;

namespace CSBlog.Core.Repository;

public interface IUserRepository
{
  /// <summary>
  /// User management related methods
  /// </summary>
  /// <param name="user"></param>
  /// <returns></returns>
  ///
  ICollection<BlogUser> GetUsers();

  Task AddUser(BlogUser user);
  Task EditUser(BlogUser user);
  BlogUser UpdateUser(BlogUser user);
  BlogUser? GetUserById(string userId);
  BlogUser? GetUserByName(string userName);
  Task<BlogUser[]> GetAllUsers();
  Task DeleteUser(string userId);
}