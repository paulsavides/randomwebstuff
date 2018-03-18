using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Pisces.AwsWebApi.GoogleAuth
{
  public class GoogleUser
  {
    public string NameIdentifier { get; set; }
    public string FullName => GivenName + " " + Surname;
    public string GivenName { get; set; }
    public string Surname { get; set; }
    public string EmailAddress { get; set; }
    public bool   EmailVerified { get; set; }
    public string PictureUrl { get; set; }
    public string Locale { get; set; }

    internal static GoogleUser FromClaims(IEnumerable<Claim> claims)
    {
      var user = new GoogleUser();

      foreach (var claim in claims)
      {
        switch (claim.Type)
        {
          case ClaimsConstants.NameIdentifier:
            user.NameIdentifier = claim.Value;
            break;
          case ClaimsConstants.EmailAddress:
            user.EmailAddress = claim.Value;
            break;
          case ClaimsConstants.GivenName:
            user.GivenName = claim.Value;
            break;
          case ClaimsConstants.Surname:
            user.Surname = claim.Value;
            break;
          case ClaimsConstants.GoogleClaims.EmailVerified:
            user.EmailVerified = Convert.ToBoolean(claim.Value);
            break;
          case ClaimsConstants.GoogleClaims.PictureUrl:
            user.PictureUrl = claim.Value;
            break;
          case ClaimsConstants.GoogleClaims.Locale:
            user.Locale = claim.Value;
            break;
        }
      }

      return user;
    }
  }
}
