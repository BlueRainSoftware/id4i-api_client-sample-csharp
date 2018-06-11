using System;
using System.Text;
using BlueRain.ID4i.Api;
using BlueRain.ID4i.Client;
using BlueRain.ID4i.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Example
{
    public class Program
    {
        static void Main(string[] args)
        {
            string authorization = Authorization();
            Configuration.Default.AddApiKey("Authorization", authorization);
            Configuration.Default.AddApiKeyPrefix("Authorization", "Bearer");
            Configuration.Default.BasePath = "https://sandbox.id4i.de";

            var apiInstance = new MetaInformationApi();

            try
            {
                // Retrieve version information about ID4i
                AppInfoPresentation result = apiInstance.ApplicationInfo();
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when calling MetaInformationApi.ApplicationInfo: " + e.Message );
            }
        }

     static string Authorization()
    {
        string apiKey = Environment.GetEnvironmentVariable("ID4I_API_KEY"); 
        string apiSecret = Environment.GetEnvironmentVariable("ID4I_API_SECRET");
        if (apiKey == null || apiSecret == null)
        {
            Console.WriteLine("ID4I_API_KEY or ID4I_API_SECRET not set in environment");
            Environment.Exit(-1);
        }

        var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret));

        SigningCredentials credentials = new SigningCredentials
            (secureKey, SecurityAlgorithms.HmacSha512); // SecurityAlgorithms.HmacSha512 = HS512 | SecurityAlgorithms.RsaSha256 = RS256

        var header = new JwtHeader(credentials);
        header.Remove("typ");
        header.Add("typ", "API"); // force typ to be "API"

        var payload = new JwtPayload
        {
            {"sub", apiKey},
            {"iat", DateTime.UtcNow},
        };

        var secToken = new JwtSecurityToken(header, payload);
        var handler = new JwtSecurityTokenHandler();

        var tokenString = handler.WriteToken(secToken);
        return tokenString;
    }
    }
}