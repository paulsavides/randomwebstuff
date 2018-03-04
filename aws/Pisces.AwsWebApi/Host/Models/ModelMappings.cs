using System.Collections.Generic;
using System.Linq;
using Pisces.AwsWebApi.Data.Models;

namespace Pisces.AwsWebApi.Host.Models
{
  public static class ModelMappings
  {
    public static QuestionModel ToModel(this Question question)
    {
      return new QuestionModel
      {
        QuestionId = question.QuestionId,
        QuestionText = question.QuestionText
      };
    }

    public static IEnumerable<QuestionModel> ToModel(this IEnumerable<Question> questions)
    {
      return questions.Select(ToModel);
    }

    public static Question ToDto(this QuestionInputModel question)
    {
      return new Question
      {
        QuestionText = question.QuestionText,
        QuestionHint = question.QuestionHint,
        AnswerText = question.AnswerText
      };
    }
  }
}