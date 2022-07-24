using CSBlog.Models.Blog;

namespace CSBlog.Core.Repository;

public interface IUnitOfWork //<T> where T : class
{
  IUserRepository User { get; }

  IRoleRepository Role { get; }

  // IBlogRepository Blog { get; }
  IRepository<Tag> Tag { get; }
  IRepository<Article> Article { get; }
  IRepository<Comment> Comment { get; }
}