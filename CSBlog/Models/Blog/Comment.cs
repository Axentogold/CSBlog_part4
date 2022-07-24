namespace CSBlog.Models.Blog;

public class Comment
{
  public string? Id { get; set; } = Guid.NewGuid().ToString();
  public string? ArticleId { get; set; }

  public string? UserId { get; set; }

  // [Required (ErrorMessage = "This field is required.")]
  // [StringLength(2048)]
  public string? Text { get; set; }
  public DateTime Added { get; set; } = DateTime.UtcNow;
  public DateTime Changed { get; set; }
}