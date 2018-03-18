using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Pisces.AwsWebApi.GoogleAuth
{
  public static class GoogleIdTokenValidator
  {
    public static GoogleUser ValidateToken(string idToken)
    {
      var jwtHandler = new JwtSecurityTokenHandler();

      var claims = jwtHandler.ValidateToken(idToken, TokenValidationParameters, out var validatedToken);

      if (validatedToken != null)
      {
        return GoogleUser.FromClaims(claims.Claims);
      }

      throw new Exception("Could not validate google identity.");
    }

    private static readonly TokenValidationParameters TokenValidationParameters = new TokenValidationParameters
    {
      ValidateActor = false, // check the profile ID

      ValidateAudience = true,
      ValidAudience = ConfigurationManager.AppSettings["GoogleClientId"],

      ValidateIssuer = true, // check token came from Google
      ValidIssuers = new List<string> { "accounts.google.com", "https://accounts.google.com" },

      ValidateIssuerSigningKey = true,
      RequireSignedTokens = true,
      IssuerSigningKeyResolver = GoogleSigningKeyResolver,
      ValidateLifetime = true,
      RequireExpirationTime = true,
      ClockSkew = TimeSpan.FromHours(13)
    };

    private static IEnumerable<SecurityKey> GoogleSigningKeyResolver(string token, SecurityToken securityToken,
      string kid, TokenValidationParameters validationParameters)
    {
      var certificates = GoogleCertRepo.Certs;

      if (certificates.ContainsKey(kid))
      {
        yield return new X509SecurityKey(certificates[kid]);
      }

    }
  }
}
