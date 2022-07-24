namespace API.Contracts.Comments;

public class GetCommentsResponse
{
  public int CommentsTotal { get; set; }
  public List<CommentView>? Comments { get; set; }
}

public class CommentView
{
  public string? Id { get; set; }
  public string? Text { get; set; }
  public DateTime Added { get; set; }
  public DateTime Changed { get; set; }
}