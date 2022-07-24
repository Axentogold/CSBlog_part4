namespace API.Contracts.Comments;

public class AddCommentRequest
{
  public string? Author { get; set; }
  public string? Text { get; set; }
}