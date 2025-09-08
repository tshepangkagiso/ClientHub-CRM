namespace EmployeeWebApp.Models;

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
        CommentID = commentID;
        DateOfComment = dateOfComment;
        SentByUserWithEmail = sentByEmail;
        SentToUserWithEmail = sentToEmail;
        Message = message;
        
    }

    public Comments(string sentByEmail, string sentToEmail, string message)
    {
        DateOfComment = DateTime.UtcNow;
        SentByUserWithEmail = sentByEmail;
        SentToUserWithEmail = sentToEmail;
        Message = message;
        
    }
}
