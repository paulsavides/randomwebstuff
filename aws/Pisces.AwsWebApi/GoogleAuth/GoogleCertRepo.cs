using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;

namespace Pisces.AwsWebApi.GoogleAuth
{
  internal static class GoogleCertRepo
  {
    private static readonly Dictionary<string, X509Certificate2> _certs = new Dictionary<string, X509Certificate2>();
    private static DateTime _certExpiration = DateTime.Now;

    internal static Dictionary<string, X509Certificate2> Certs
    {
      get
      {
        if (ShouldRefresh())
        {
          RefreshCert();
        }

        return _certs;
      }
    }

    private static bool ShouldRefresh()
    {
      return _certs.Count == 0 || DateTime.Now > _certExpiration;
    }

    private static void RefreshCert()
    {
      _certs.Clear();

      var req = WebRequest.Create("https://www.googleapis.com/oauth2/v1/certs");
      var resp = req.GetResponse();

      if (resp == null)
      {
        throw new Exception("Failure requesting google certs.");
      }

      // update expiration time on our certs
      var strExpiration = resp.Headers["expires"] ?? "";
      _certExpiration = DateTime.TryParse(strExpiration, out var expirationTime) ?
          expirationTime : DateTime.Now.AddHours(2);

      var respStream = resp.GetResponseStream();
      if (respStream == null)
      {
        throw new Exception("Google isn't cooperating.");
      }

      // load up the response, generally not a large response so
      // no need to do anything fancy
      var sr = new StreamReader(respStream);
      var respAsString = sr.ReadToEnd();
      sr.Close();

      // response comes back as json { "key" : "val", etc }
      var certs = JsonConvert.DeserializeObject<Dictionary<string, string>>(respAsString);

      var encoding = new UTF8Encoding();
      foreach (var cert in certs)
      {
        _certs.Add(cert.Key, new X509Certificate2(encoding.GetBytes(cert.Value)));
      }
    }

  }
}
