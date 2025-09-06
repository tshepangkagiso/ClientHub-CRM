namespace CRM_API.Models;

public class Comments
{
    [Key]
    public int CommentID { get; set; }

    [Required]
    public DateTime DateOfComment { get; set; } = DateTime.UtcNow;

    [Required]
    public string SentByUserWithEmail { get; set; } = string.Empty;

    [Required]
    public string SentToUserWithEmail { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public Comments() { }
    public Comments(int commentID, DateTime dateOfComment, string sentByEmail, string sentToEmail, string message)
    {
        this.CommentID = commentID;
        this.DateOfComment = dateOfComment;
        this.SentByUserWithEmail = sentByEmail;
        this.SentToUserWithEmail = sentToEmail;
        this.Message = message;
        
    }

    public Comments(string sentByEmail, string sentToEmail, string message)
    {
        this.DateOfComment = DateTime.UtcNow;
        this.SentByUserWithEmail = sentByEmail;
        this.SentToUserWithEmail = sentToEmail;
        this.Message = message;
        
    }
}
