using SharedModels.Models;
using MeetingMicroService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingMicroService.Controllers
{
    [ApiController]
    [Route("api/meetings")]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingService _meetingService;

        public MeetingController(MeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet]
        public IActionResult GetMeetings()
        {
            return Ok(_meetingService.GetMeetings());
        }

        [HttpPost("create")]
        [Authorize] // Авторизация обязательна
        public IActionResult CreateMeeting([FromBody] CreateMeetingRequest meeting)
        {
            var creator = User.Identity?.Name;
            if (string.IsNullOrEmpty(creator))
            {
                return Unauthorized("Не удалось получить данные пользователя");
            }

            _meetingService.CreateMeeting(meeting.Title, DateTime.Now, creator);
            return Ok("Встреча создана");
        }


        // Измененный метод для регистрации с использованием залогиненного пользователя
        [HttpPost("register")]
        [Authorize] // Обязательно, чтобы пользователи были аутентифицированы
        public IActionResult Register([FromQuery] RegisterMeetingRequest meetingRequest)
        {
            // Получаем имя залогиненного пользователя из JWT токена
            var participant = User.Identity?.Name; // Имя пользователя из токена
            if (string.IsNullOrEmpty(participant))
            {
                return Unauthorized("Не удалось получить данные пользователя");
            }

            // Регистрация пользователя на встречу
            if (!_meetingService.RegisterForMeeting(meetingRequest.MeetingId, participant))
            {
                return BadRequest("Не удалось зарегистрироваться на встречу");
            }
            return Ok("Регистрация успешна");
        }
    }
}
