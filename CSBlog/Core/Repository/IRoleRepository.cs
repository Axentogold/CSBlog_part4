using Microsoft.AspNetCore.Identity;

namespace CSBlog.Core.Repository;

public interface IRoleRepository
{
  ICollection<IdentityRole> GetRoles();
}