namespace API.Contracts.Users;

public class EditUserRequest
{  
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
}