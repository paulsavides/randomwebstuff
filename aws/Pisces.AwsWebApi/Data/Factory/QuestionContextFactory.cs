using Pisces.AwsWebApi.Data.Context;

namespace Pisces.AwsWebApi.Data.Factory
{
  public class QuestionContextFactory : IDbContextFactory<QuestionContext>
  {
    public QuestionContext CreateContext()
    {
      return new QuestionContext();
    }
  }
}
