using CSBlog.Core.Repository;
using CSBlog.Models.Blog;

namespace CSBlog.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
  public IUserRepository User { get; }
  public IRoleRepository Role { get; }

  public IRepository<Tag> Tag { get; }

  public IRepository<Article> Article { get; }
  public IRepository<Comment> Comment { get; }


  // public IBlogRepository Blog { get; }

  public UnitOfWork(IUserRepository user, IRoleRepository role, IRepository<Tag> tag, IRepository<Article> article,
    IRepository<Comment> comment)
  {
    User = user;
    Role = role;
    Tag = tag;
    Article = article;
    Comment = comment;
  }
}