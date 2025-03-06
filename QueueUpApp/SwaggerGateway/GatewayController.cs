using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

[Route("api/gateway")]
[ApiController]
public class GatewayController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public GatewayController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // 1️ Регистрация пользователя + отправка email
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest userData)
    {
        // Вызываем сервис авторизации
        var authResponse = await _httpClient.PostAsJsonAsync("http://localhost:5001/api/auth/register", userData);
        if (!authResponse.IsSuccessStatusCode) return StatusCode((int)authResponse.StatusCode, await authResponse.Content.ReadAsStringAsync());

        // Извлекаем email пользователя
        UserResponse userResponse = await authResponse.Content.ReadFromJsonAsync<UserResponse>();
        EmailRequest emailRequest = new EmailRequest(userResponse.Email,"Регистрация пройдена",  "Вы успешно зарегистрированы!");

        // Отправляем email пользователю
        var mailResponse = await _httpClient.PostAsJsonAsync("http://localhost:5002/api/mail/send", emailRequest);

        return Ok(new { message = "User registered, email sent", emailStatus = mailResponse.StatusCode });
    }

    [HttpPost("meeting/create")]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequest meetingData)
    {
        // Создаём встречу
        var meetingResponse = await _httpClient.PostAsJsonAsync("http://localhost:5003/api/meetings/create", meetingData);
        if (!meetingResponse.IsSuccessStatusCode) return StatusCode((int)meetingResponse.StatusCode, await meetingResponse.Content.ReadAsStringAsync());

        // Извлекаем email владельца встречи
        var meeting = await meetingResponse.Content.ReadFromJsonAsync<MeetingResponse>();
        EmailRequest emailData = new EmailRequest(meeting.OwnerEmail, "Создание встречи" , "Встреча успешно создана!" );

        // Отправляем email владельцу
        var mailResponse = await _httpClient.PostAsJsonAsync("http://localhost:5002/api/mail/send", emailData);

        return Ok(new { message = "Meeting created, email sent", emailStatus = mailResponse.StatusCode });
    }

    // 3️ Регистрация на встречу + отправка email пользователю и владельцу
    [HttpPost("meeting/register")]
    public async Task<IActionResult> RegisterForMeeting([FromBody] RegisterMeetingRequest registrationData)
    {
        // Регистрируем пользователя на встречу
        var registrationResponse = await _httpClient.PostAsJsonAsync("http://localhost:5003/api/meetings/register", registrationData);
        if (!registrationResponse.IsSuccessStatusCode) return StatusCode((int)registrationResponse.StatusCode, await registrationResponse.Content.ReadAsStringAsync());

        // Извлекаем данные о встрече
        var meeting = await registrationResponse.Content.ReadFromJsonAsync<MeetingResponse>();

        // Отправляем email владельцу встречи
        EmailRequest ownerEmail =new EmailRequest (meeting.OwnerEmail, "Встреча", "Новый участник зарегистрировался на встречу!" );
        var ownerEmailResponse = await _httpClient.PostAsJsonAsync("http://localhost:5002/api/mail/send", ownerEmail);

        // Отправляем email участнику

        EmailRequest memberEmail = new EmailRequest(meeting.OwnerEmail, "Встреча", "Вы успешно зарегистрированы на встречу!");
        var userEmailResponse = await _httpClient.PostAsJsonAsync("http://localhost:5002/api/mail/send", memberEmail);

        return Ok(new { message = "User registered for meeting, emails sent", emailStatus = new { owner = ownerEmailResponse.StatusCode, user = userEmailResponse.StatusCode } });
    }
}
