namespace API.Contracts.Users;

public class GetUsersResponse
{
  public int UsersTotal { get; set; }
  public List<UserView>? Users { get; set; }
}

public class UserView
{
  public string FirstName { get; set; } = string.Empty;

  public string LastName { get; set; } = string.Empty;

  public string UserName { get; set; } = string.Empty;

  public DateTime Registered { get; set; }

  public string GetFullName()
  {
    return $"{FirstName} {LastName}";
  }
}