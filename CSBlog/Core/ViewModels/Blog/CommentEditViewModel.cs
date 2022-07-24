using System.ComponentModel.DataAnnotations;

namespace CSBlog.Core.ViewModels.Blog;

public class CommentEditViewModel
{
  [Required]
  [Display(Name = "Your commentary")]
  [StringLength(1024, MinimumLength = 3, ErrorMessage = "Your commentary is too short or too long")]
  public string? Text { get; set; }

  // [Required]
  // public string? CommentId { get; set; }
  // [Required]
  public string? ArticleId { get; set; }
  // public Article? Article { get; set; }
}