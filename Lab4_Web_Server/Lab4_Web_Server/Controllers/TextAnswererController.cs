using BertModelLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Text.Json.Nodes;
namespace Lab4_Web_Server.Controllers
{
    [Route("api/textQuestionAnswerer")]
    [ApiController]
    public class TextAnswererController : Controller
    {
        private string modelWebSource = "https://storage.yandexcloud.net/dotnet4/bert-large-uncased-whole-word-masking-finetuned-squad.onnx";
        private BertModelService bertModelService;

        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private CancellationToken token;

        public TextAnswererController(BertModelService bertModelService) 
        {
            this.bertModelService = bertModelService;
            token = cancelTokenSource.Token;
        }

        [HttpPost]
        public async Task<IActionResult> GetAnswers([FromBody] TextAndQuestionsRequest request)
        {
            try
            {
                string text = request.text;
                if (text == "")
                    return BadRequest("Empty text!");
                List<QuestionAndAnswerId> questionsAndAnswerIds = request.questionsAndAnswerIds;
                List<AnswerResponse> answerIdAndvalue = new List<AnswerResponse>();
                List<Task<AnswerResponse>> Tasks = new List<Task<AnswerResponse>>();
                foreach (var item in questionsAndAnswerIds)
                {
                    string question = item.question;
                    string answerId = item.answerId;
                    if (question == "")
                       return BadRequest("Empty text!");
                    if (question == "cancel") { cancelTokenSource.Cancel(); }
                    Tasks.Add(bertModelService.ProcessQuestionAsync(text, question, answerId, token));
                }
                var answerResponses = await Task.WhenAll(Tasks);

                return Ok(answerResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        public record class TextAndQuestionsRequest(string text, List<QuestionAndAnswerId> questionsAndAnswerIds);
        public record class QuestionAndAnswerId(string question, string answerId);
        public record class AnswerResponse(string answerId, string answer);
    }
}
