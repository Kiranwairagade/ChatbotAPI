using Microsoft.AspNetCore.Mvc;
using ChatbotAPI.Services;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public IActionResult GetAnswer([FromBody] QuestionModel question)
        {
            string answer = _chatbotService.GetAnswer(question.Question);
            return Ok(new { answer });
        }
    }

    public class QuestionModel
    {
        public string Question { get; set; }
    }
}
