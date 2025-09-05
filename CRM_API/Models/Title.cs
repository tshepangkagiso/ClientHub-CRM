namespace CRM_API.Models;

public class Titles
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;

    public Titles(string title)
    {
        this.Title = title;
    }
}
