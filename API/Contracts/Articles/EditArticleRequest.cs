using API.Contracts.Tags;

namespace API.Contracts.Articles;

public class EditArticleRequest
{
  public string? Title { get; set; }

  public string? Text { get; set; }

  public ICollection<TagView> Tags { get; set; }
}