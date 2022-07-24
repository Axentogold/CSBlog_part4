using System.ComponentModel.DataAnnotations;

namespace CSBlog.Core.ViewModels.Blog;

public class TagViewModel
{
  // public Tag BlogTag { get; set; } = null!;
  [Required]
  [StringLength(16, MinimumLength = 2, ErrorMessage = "Tag name must be string between 2 and 16 characters long")]
  [Display(Name = "Tag")]
  public string? TagName { get; set; }

  // public IList<SelectListItem> TagNames { get; set; } = null!;
}