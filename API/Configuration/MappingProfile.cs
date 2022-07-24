using API.Contracts.Articles;
using API.Contracts.Comments;
using API.Contracts.Tags;
using API.Contracts.Users;
using AutoMapper;
using CSBlog.Models.Blog;
using CSBlog.Models.User;

namespace API.Configuration;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Tag, TagView>();
    CreateMap<TagView, Tag>();
    CreateMap<Article, ArticleView>();
    CreateMap<Comment, CommentView>();
    CreateMap<BlogUser, UserView>();

    CreateMap<AddTagRequest, Tag>();
    CreateMap<AddArticleRequest, Article>();
    CreateMap<AddCommentRequest, Comment>();
    CreateMap<AddUserRequest, BlogUser>();

  }
}