using System.Configuration;
using System.Web.Http;
using WebActivatorEx;
using Pisces.AwsWebApi.Host;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Pisces.AwsWebApi.Host
{
  public class SwaggerConfig
  {
    public static void Register()
    {
      var thisAssembly = typeof(SwaggerConfig).Assembly;

      GlobalConfiguration.Configuration
        .EnableSwagger(c =>
        {
          c.RootUrl(req => ConfigurationManager.AppSettings["SwaggerBaseUrl"]);
          c.Schemes(new[] { ConfigurationManager.AppSettings["SwaggerEnabledProtocal"] });
          c.SingleApiVersion("v1", "Question Api");
        })
        .EnableSwaggerUi(c => { });
    }
  }
}
