using SharedModels.Models;

namespace MeetingMicroService.Services
{
    public class MeetingService
    {
        private readonly List<Meeting> _meetings = new();
        private readonly string? _meetingFilePath = "meeting.json";
        private readonly JsonSaveService _jsonSaveService;

        public MeetingService()
        {
            _jsonSaveService = new JsonSaveService();
            _meetings = _jsonSaveService.Load<List<Meeting>>(_meetingFilePath);
        }

        public List<Meeting> GetMeetings() => _meetings;

        public Meeting? GetMeetingById(string id) => _meetings.FirstOrDefault(m => m.GUID == id);

        public void CreateMeeting(string title, DateTime date, string creator)
        {
            _meetings.Add(new Meeting(Guid.NewGuid().ToString(), title, date, creator));
            _jsonSaveService.Save(_meetings, _meetingFilePath);
        }


        // Измененный метод для регистрации на встречу, использующий имя из JWT токена
        public bool RegisterForMeeting(string meetingId, string participant)
        {
            var meeting = GetMeetingById(meetingId);
            if (meeting == null || meeting.Members.Contains(participant))
                return false;
            meeting.Members.Add(participant);
            _jsonSaveService.Save(_meetings, _meetingFilePath); // Сохраняем после регистрации
            return true;
        }
    }
}
