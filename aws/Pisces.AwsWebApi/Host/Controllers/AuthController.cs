using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Pisces.AwsWebApi.GoogleAuth;
using Pisces.AwsWebApi.Host.Models;

namespace Pisces.AwsWebApi.Host.Controllers
{
  [RoutePrefix("api/Auth")]
  public class AuthController : ApiController
  {
    [ResponseType(typeof(IEnumerable<UserModel>))]
    [HttpPost]
    public HttpResponseMessage ProcessToken(TokenModel tokenModel)
    {
      var user = GoogleIdTokenValidator.ValidateToken(tokenModel.IdToken);
      return Request.CreateResponse(HttpStatusCode.OK, new UserModel
      {
        GivenName = user.GivenName,
        Surname = user.Surname,
        EmailAddress = user.EmailAddress,
        PictureUrl = user.PictureUrl
      });
    }
  }
}