using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Pisces.AwsWebApi.Data.Factory;
using Pisces.AwsWebApi.Host.Models;

namespace Pisces.AwsWebApi.Host.Controllers
{
  [RoutePrefix("api/Question")]
  public class QuestionController : ApiController
  {
    private readonly QuestionContextFactory _contextFactory;

    public QuestionController(QuestionContextFactory contextFactory)
    {
      _contextFactory = contextFactory;
    }


    [ResponseType(typeof(IEnumerable<QuestionModel>))]
    [HttpGet]
    public HttpResponseMessage GetQuestions()
    {
      IEnumerable<QuestionModel> resp;
      using (var ctx = _contextFactory.CreateContext())
      {
        resp = ctx.Questions.ToModel().ToList();
      }

      return Request.CreateResponse(HttpStatusCode.OK, resp);
    }

    [ResponseType(typeof(QuestionModel))]
    [HttpGet]
    public HttpResponseMessage GetQuestion(int id)
    {
      QuestionModel question;
      using (var ctx = _contextFactory.CreateContext())
      {
        question = ctx.Questions.Find(id)?.ToModel();
      }

      return question == null ?
        Request.CreateResponse(HttpStatusCode.NotFound, new SimpleMessageResponseModel($"No question with id {id} exists.")) :
        Request.CreateResponse(HttpStatusCode.OK, question);
    }


    [ResponseType(typeof(QuestionModel))]
    [Route("Random")]
    [HttpGet]
    public HttpResponseMessage GetRandomQuestion()
    {
      QuestionModel question = null;
      using (var ctx = _contextFactory.CreateContext())
      {
        if (ctx.Questions?.Count() > 0)
        {
          question = ctx.Questions.OrderBy(r => Guid.NewGuid()).Take(1).First().ToModel();
        }
      }

      return question == null ?
        Request.CreateResponse(HttpStatusCode.NotFound, new SimpleMessageResponseModel("No questions exist.")) :
        Request.CreateResponse(HttpStatusCode.OK, question);

    }

    
    [ResponseType(typeof(QuestionInputResultModel))]
    [HttpPost]
    public HttpResponseMessage PostQuestion(QuestionInputModel question)
    {
      var dto = question.ToDto();
      using (var ctx = _contextFactory.CreateContext())
      {
        ctx.Questions.Add(dto);
        ctx.SaveChanges();
      }

      return Request.CreateResponse(HttpStatusCode.Created,
        new QuestionInputResultModel
        {
          AnswerText = dto.AnswerText,
          QuestionText = dto.QuestionText,
          QuestionHint = dto.QuestionHint,
          QuestionId = dto.QuestionId
        });
    }

    [ResponseType(typeof(QuestionWithHintModel))]
    [Route("{id}/Hint")]
    [HttpGet]
    public HttpResponseMessage GetQuestionHint(int id)
    {
      QuestionWithHintModel questionHint = null;

      using (var ctx = _contextFactory.CreateContext())
      {
        var questionDto = ctx.Questions.Find(id);

        if (questionDto != null)
        {
          questionHint = new QuestionWithHintModel
          {
            QuestionId = questionDto.QuestionId,
            QuestionHint = questionDto.QuestionHint,
            QuestionText = questionDto.QuestionText
          };
        }
      }

      return questionHint == null
        ? Request.CreateResponse(HttpStatusCode.NotFound, new SimpleMessageResponseModel($"No question with id {id} found."))
        : Request.CreateResponse(HttpStatusCode.OK, questionHint);
    }

    [ResponseType(typeof(AnswerResultModel))]
    [Route("{id}/Answer")]
    [HttpGet]
    public HttpResponseMessage AnswerQuestion(int id, string answer)
    {
      AnswerResultModel result = null;
      using (var ctx = _contextFactory.CreateContext())
      {
        var question = ctx.Questions.Find(id);
        if (question != null)
        {
          result = new AnswerResultModel
          {
            Correct = string.Equals(question.AnswerText, answer, StringComparison.CurrentCultureIgnoreCase)
          };
        }
      }

      return result == null ?
        Request.CreateResponse(HttpStatusCode.NotFound, new SimpleMessageResponseModel($"No question exists with id {id} to be answered.")) :
        Request.CreateResponse(HttpStatusCode.OK, result);
    }
  }
}