using API.Contracts.Tags;

namespace API.Contracts.Articles;

public class AddArticleRequest
{
  public string Author { get; set; } = string.Empty;

  public string? Title { get; set; }
  
  public string? Text { get; set; }

  public ICollection<TagView> Tags { get; set; }

}