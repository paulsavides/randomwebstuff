namespace Pisces.AwsWebApi.Host.Models
{
  public class SimpleMessageResponseModel
  {
    public SimpleMessageResponseModel(string message)
    {
      Message = message;
    }

    public string Message { get; set; }
  }
}