using System.Data.Entity;

namespace Pisces.AwsWebApi.Data.Factory
{
  public interface IDbContextFactory<out TDbContext> where TDbContext : DbContext
  {
    TDbContext CreateContext();
  }
}
