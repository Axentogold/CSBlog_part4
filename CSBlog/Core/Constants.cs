namespace CSBlog.Core;

public static class Constants
{
  public static class Roles
  {
    public const string Administrator = "Administrator";
    public const string Moderator = "Moderator";
    public const string Author = "Author";
    public const string User = "User";
  }

  public static class Policies
  {
    public const string RequireAdmin = "RequireAdmin";
    public const string RequireModerator = "RequireModerator";
  }
}