namespace CRM_API.Models;

public class ProfilePicture
{
    public string Name { get; set; } = string.Empty;
    public string Type {  get; set; } = string.Empty;
    public Stream? Content { get; set; }

    public ProfilePicture(string fileName, string contentType, Stream OpenReadStream)
    {
        this.Name = fileName;
        this.Type = contentType;
        this.Content = OpenReadStream;
    }
}
