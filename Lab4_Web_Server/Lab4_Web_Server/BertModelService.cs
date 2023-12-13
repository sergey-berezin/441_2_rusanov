using BertModelLibrary;
using static Lab4_Web_Server.Controllers.TextAnswererController;

namespace Lab4_Web_Server
{
    public class BertModelService
    {
        private string modelWebSource = "https://storage.yandexcloud.net/dotnet4/bert-large-uncased-whole-word-masking-finetuned-squad.onnx";
        private BertModel bertModel;
        public BertModelService() { }
        public async void GetBertModel()
        {
            try
            {
                bertModel = new BertModel(modelWebSource);
                var createTask = bertModel.Create();
                await createTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<AnswerResponse> ProcessQuestionAsync(string text, string question, string answerId, CancellationToken token)
        {
            try
            {
                var answer = await Task.Run(() => bertModel.AnswerQuestionAsync(text, question, token));
                return new AnswerResponse(answerId, answer.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return new AnswerResponse(answerId, ex.Message);
            }
        }
    }
}
