namespace Pisces.AwsWebApi.GoogleAuth
{
  internal static class ClaimsConstants
  {
    internal const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    internal const string EmailAddress   = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    internal const string GivenName      = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
    internal const string Surname        = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

    internal static class GoogleClaims
    {
      internal const string EmailVerified = "email_verified";
      internal const string PictureUrl    = "picture";
      internal const string Locale        = "locale";
    }
  }
}
