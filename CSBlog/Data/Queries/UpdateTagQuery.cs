namespace CSBlog.Data.Queries;

public class UpdateTagQuery
{
  public UpdateTagQuery(string newTagName)
  {
    NewTagName = newTagName;
  }

  public string NewTagName { get; }
}