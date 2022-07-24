using System.ComponentModel.DataAnnotations.Schema;

namespace CSBlog.Models.Blog;

public class Article
{
  public Article()
  {
    // Comments = new HashSet<Comment>();
    Tags = new HashSet<Tag>()!;
  }

  public string? Id { get; init; } = Guid.NewGuid().ToString();
  public string UserId { get; set; } = string.Empty;

  public string Author { get; set; } = string.Empty;

  // [Required (ErrorMessage = "This field is required.")]
  // [StringLength(64)]
  public string? Title { get; set; }

  // [Required (ErrorMessage = "This field is required.")]
  public string? Text { get; set; }
  public ICollection<Tag?> Tags { get; set; }

  // public ICollection<ArticleTag>? ArticleTags { get; set; }

  public ICollection<Comment>? Comments { get; set; }

  // public List<ArticleComment>? ArticleComments  { get; set; }
  public DateTime Published { get; init; } = DateTime.UtcNow;
  public DateTime Edited { get; set; }
}

public class ArticleTag
{ 
  public string Id { get; set; } = Guid.NewGuid().ToString();
  
  [ForeignKey(nameof(Article))]
  public string? ArticleId { get; init; }
  public Tag Tag { get; set; } = null!;

  [ForeignKey(nameof(Tag))]
  public string? TagId { get; init; }
  public Article Article { get; set; } = null!;
}
