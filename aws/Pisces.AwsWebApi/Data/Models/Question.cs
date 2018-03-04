using System.ComponentModel.DataAnnotations;

namespace Pisces.AwsWebApi.Data.Models
{
  public class Question
  {
    [Key]
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public string QuestionHint { get; set; }
    public string AnswerText { get; set; }
  }
}
