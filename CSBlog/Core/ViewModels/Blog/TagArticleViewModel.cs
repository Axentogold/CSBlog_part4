using System.ComponentModel.DataAnnotations;
using CSBlog.Models.Blog;

namespace CSBlog.Core.ViewModels.Blog;

public class TagArticleViewModel
{  
  [Required] 
  public string? TagName { get; set; }
  [Required] 
  public string? TagId { get; set; }
  public Tag? Tag { get; set; }

}