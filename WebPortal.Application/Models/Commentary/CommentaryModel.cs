namespace WebPortal.Application.Models.Commentary;

public class CommentaryModel
{
    public Guid Id { get; set; }
    public string AuthorNickName { get; set; }
    public string? AuthorAvatar { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
    public int CountLikes { get; set; }
    public int CountDislikes { get; set; }
    public ICollection<CommentaryModel> Replies { get; set; }
}