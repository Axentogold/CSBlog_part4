using CSBlog.Models.Blog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSBlog.Core.ViewModels.Blog;

public class ArticleViewModel
{
  public Article? Article { get; set; }

  public IList<SelectListItem>? Tags { get; set; }
}