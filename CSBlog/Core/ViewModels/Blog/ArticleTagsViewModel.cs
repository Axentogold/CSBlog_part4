using CSBlog.Models.Blog;

namespace CSBlog.Core.ViewModels.Blog;

public class ArticleTagsViewModel
{
  public ICollection<Article>? Articles { get; set; } 

  public List<ICollection<Tag>?>? Tags { get; set; }
  // public ICollection<ArticleTag> ArticleTags { get; set; }
}