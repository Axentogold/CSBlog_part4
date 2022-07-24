namespace CSBlog.Models.Blog;

public class Tag
{
  public Tag()
  {
    Articles = new HashSet<Article>();
  }

  public string? Id { get; init; } = Guid.NewGuid().ToString();

  public ICollection<Article> Articles { get; set; }

  // public ICollection<ArticleTag>? ArticleTags { get; set; }
  public string? TagName { get; set; } = string.Empty;
}