namespace WebPortal.Application.Models.Commentary;

public static class CommentaryTreeMapper
{
    public static IEnumerable<CommentaryModel> MapToTree(IEnumerable<Domain.Commentary> commentaries)
    {
        var parents = commentaries
            .Where(commentary => commentary.Parent == null)
            .Select(commentary => new CommentaryModel
            {
                AuthorNickName = commentary.Author.NickName,
                CountDislikes = commentary.CountDislikes,
                CountLikes = commentary.CountLikes,
                CreationDate = commentary.CreationDate,
                AuthorAvatar = commentary.Author.Avatar,
                Id = commentary.Id,
                Text = commentary.Text,
                Replies = new List<CommentaryModel>(
                    commentaries
                        .Where(c => c.Parent!= null && c.Parent.Id == commentary.Id)
                        .Select(Map))
            });
        
        return parents;
    }

    private static CommentaryModel Map(Domain.Commentary commentary)
    {
        return new CommentaryModel
        {
            AuthorNickName = commentary.Author.NickName,
            CountDislikes = commentary.CountDislikes,
            CountLikes = commentary.CountLikes,
            CreationDate = commentary.CreationDate,
            AuthorAvatar = commentary.Author.Avatar,
            Id = commentary.Id,
            Text = commentary.Text
        };
    }
}