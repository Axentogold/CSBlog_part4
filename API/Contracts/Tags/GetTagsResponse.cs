namespace API.Contracts.Tags;

public class GetTagsResponse
{
  public int TagsTotal { get; set; }
  public List<TagView>? Tags { get; set; }
}

public class TagView
{
  public string? TagName { get; set; }
}