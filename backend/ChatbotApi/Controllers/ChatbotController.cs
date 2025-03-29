using Microsoft.AspNetCore.Mvc;
using ChatbotApi.Models;
using ChatbotApi.Services;

namespace ChatbotApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotService _chatbotService;
        private readonly ILogger<ChatbotController> _logger;

        public ChatbotController(ChatbotService chatbotService, ILogger<ChatbotController> logger)
        {
            _chatbotService = chatbotService;
            _logger = logger;
        }

        [HttpPost("query")]
        public ActionResult<ChatbotResponse> Query([FromBody] ChatbotQuery query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.Text))
                {
                    return BadRequest("Query text cannot be empty");
                }

                _logger.LogInformation($"Processing query: {query.Text}");
                
                var result = _chatbotService.ProcessQuery(query.Text);
                return Ok(new ChatbotResponse
                {
                    Intent = result.Intent,
                    Confidence = result.Confidence,
                    Response = _chatbotService.GenerateResponse(result.Intent, query.Text)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chatbot query");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("intents")]
        public ActionResult<IEnumerable<string>> GetIntents()
        {
            try
            {
                var intents = _chatbotService.GetSupportedIntents();
                return Ok(intents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supported intents");
                return StatusCode(500, "An error occurred while retrieving supported intents");
            }
        }
    }
}