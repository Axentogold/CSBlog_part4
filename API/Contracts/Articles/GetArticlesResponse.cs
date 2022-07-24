using API.Contracts.Tags;

namespace API.Contracts.Articles;

public class GetArticlesResponse
{
  public int ArticlesTotal { get; set; }
  public List<ArticleView>? Articles { get; set; }
}

public class ArticleView
{
  public string Author { get; set; } = string.Empty;

  public string? Title { get; set; }

  public string? Text { get; set; }
  
  public ICollection<TagView> Tags { get; set; }
  
  public DateTime Published { get; init; }
  
  public DateTime Edited { get; set; }
}