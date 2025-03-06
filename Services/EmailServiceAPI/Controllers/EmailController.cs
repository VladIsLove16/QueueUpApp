using EmailServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailServiceAPI.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        // Внедрение зависимостей через конструктор
        public EmailController(EmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            _logger.LogInformation($"Attempt to send email to {request.Email} with subject {request.Subject}");
            try
            {

                await _emailService.SendEmailAsync(request.Email, request.Subject, request.Message);
                _logger.LogInformation($"Email sent to {request.Email} successfully.");
                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while sending email: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}