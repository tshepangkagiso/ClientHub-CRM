namespace EmployeeWebApp.Models;

public class JwtToken
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    [JsonProperty("user_id")]
    public int UserID { get; set; }
}


