using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Movies.Api.Sdk.DemoConsumer;

public class AuthTokenProvider
{
   private readonly HttpClient _httpClient;
   private string _cachedToken = string.Empty;
   private static readonly SemaphoreSlim _lock = new(1, 1);

   public AuthTokenProvider(HttpClient httpClient)
   {
      _httpClient = httpClient;
   }

   public async Task<string> GetTokenAsync()
   {
       if (!string.IsNullOrEmpty(_cachedToken))
       {
           var jwt = new JwtSecurityTokenHandler()
               .ReadJwtToken(_cachedToken);

           var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
           var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
           if (expiryDateTime > DateTime.UtcNow)
           {
               return _cachedToken;
           }
       }

       await _lock.WaitAsync();
       var response = await _httpClient.PostAsJsonAsync("https://localhost:5003/token", new
       {
           userid = "23bc0b0d-4dac-4c02-a75d-d044ce0bed2b",
           email = "amagallon@someting.com",
           customClaims = new Dictionary<string, object>
           {
               { "admin", true },
               { "trusted_member", true } 
           }
       });

       var newToken = await response.Content.ReadAsStringAsync();
       _cachedToken = newToken;
       _lock.Release();

       return newToken;
   }

   private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
   {
       var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
       dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

       return dateTime;
   }
}