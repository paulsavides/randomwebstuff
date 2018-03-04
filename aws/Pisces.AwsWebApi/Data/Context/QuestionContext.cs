using System.Configuration;
using System.Data.Entity;
using Pisces.AwsWebApi.Data.Models;

namespace Pisces.AwsWebApi.Data.Context
{
  public class QuestionContext : DbContext
  {
    internal QuestionContext() : base(GetRdsConnectionString()) { }

    public DbSet<Question> Questions { get; set; }

    private static string GetRdsConnectionString()
    {
      var appConfig = ConfigurationManager.AppSettings;

      if (appConfig["APP_MODE"] == "LOCAL")
      {
        return ConfigurationManager.ConnectionStrings["QuestionContext"].ConnectionString;
      }

      string dbname = appConfig["RDS_DB_NAME"];

      if (string.IsNullOrEmpty(dbname)) return null;

      string username = appConfig["RDS_USERNAME"];
      string password = appConfig["RDS_PASSWORD"];
      string hostname = appConfig["RDS_HOSTNAME"];
      string port = appConfig["RDS_PORT"];

      return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
    }
  }
}
