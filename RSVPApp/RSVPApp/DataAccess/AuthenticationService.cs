using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using RSVPApp.Models;

namespace RSVPApp.DataAccess;

public class AuthenticationService
{
    private const string BaseUrl = "https://localhost:44393/";

    public async Task<bool> RegisterUserAsync(User user)
    {
        using HttpClient client = new();

        var response = await client.PostAsJsonAsync(
            $"{BaseUrl}api/Values/register",
            user);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AuthenticateUserAsync(
        string email,
        string password)
    {
        using HttpClient client = new();

        string token = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{email}:{password}"));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", token);

        var response = await client.GetAsync(
            $"{BaseUrl}api/Values");

        return response.IsSuccessStatusCode;
    }
}
