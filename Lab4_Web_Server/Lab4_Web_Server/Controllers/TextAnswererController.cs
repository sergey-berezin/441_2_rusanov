using Microsoft.AspNetCore.Mvc;

namespace Lab4_Web_Server.Controllers
{
    public class TextAnswererController : Controller
    {
        [HttpGet]
        public async Task Index()
        {
            string content = @"<form method='post'>
                <label>Name:</label><br />
                <input name='name' /><br />
                <label>Age:</label><br />
                <input type='number' name='age' /><br />
                <input type='submit' value='Send' />
            </form>";
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync(content);
        }

        [HttpPost]
        public string Index(string name, int age) => $"{name}: {age}";
    }
}
