namespace CRM_API.Services.Security.Authority;

public static class AppRepository
{
    private static List<Application> _applications = new List<Application>()
    {
        new Application
        {
            ApplicationID = 1,
            ApplicationName = "EmployeeApp",
            Username = "Employee",
            Password = "1234",
            Scopes = "read,write"
        },
        new Application
        {
            ApplicationID = 2,
            ApplicationName = "ClientApp",
            Username = "Client",
            Password = "1234",
            Scopes = "read,write"
        }
    };

    public static Application? GetApplicationByUsername(string username)
    {
        return _applications.FirstOrDefault(x => x.Username == username);
    }
}
