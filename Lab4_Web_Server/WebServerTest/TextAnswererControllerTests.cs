using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using static Lab4_Web_Server.Controllers.TextAnswererController;

namespace WebServerTest
{
    public class TextAnswererControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;
        public TextAnswererControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }


        [Fact]
        public async Task GetAnswersTest()
        {
            var client = factory.CreateClient();

            string text = File.ReadAllText("..\\..\\..\\Resources\\hobbit.txt");

            QuestionAndAnswerId question1 = new QuestionAndAnswerId("what is it?", "answer1");
            QuestionAndAnswerId question2 = new QuestionAndAnswerId("what is a hobbit?", "answer2");
            QuestionAndAnswerId question3 = new QuestionAndAnswerId("what do hobbits look like?", "answer3");
            QuestionAndAnswerId question4 = new QuestionAndAnswerId("what did hobbits dress?", "answer4");
            List<QuestionAndAnswerId> questions = new List<QuestionAndAnswerId> { question1, question2, question3, question4 };
            TextAndQuestionsRequest request = new TextAndQuestionsRequest(
                text,
                questions
            );

            var inputJson = JsonConvert.SerializeObject(request);
            var response = await client.PostAsJsonAsync("https://localhost:7023/api/textQuestionAnswerer", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var answersJson = await response.Content.ReadAsStringAsync();
            var answersObjects = JsonConvert.DeserializeObject<List<AnswerResponse>>(answersJson);
            Assert.Equal(questions.Count, answersObjects.Count);
            for (int i = 0; i < answersObjects.Count; i++)
            {
                Assert.Equal(questions[i].answerId, answersObjects[i].answerId);
            }
        }
    }
}