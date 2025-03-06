using AuthMicroservice.Services;
using SharedModels.Models;
namespace AuthMicroservice.Services
{
    public class UserService
    {
        private readonly JsonSaveService _jsonSaveService;
        private List<User> _users;
        private readonly string _filePath = "users.json";
        private Dictionary<string, User> _userDict;

        public UserService()
        {
            _jsonSaveService = new JsonSaveService();
            _users = _jsonSaveService.Load<List<User>>(_filePath) ?? new List<User>();
            _userDict = _users.ToDictionary(u => u.Username, u => u);
        }

        public bool Register(string username, string password)
        {
            if (_userDict.ContainsKey(username)) 
                return false;
            var user = new User { Username = username, PasswordHash = HashPassword(password) };
            _users.Add(user);
            _userDict[username] = user;
            _jsonSaveService.Save(_users, _filePath);
            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            return _userDict.TryGetValue(username, out var user) && VerifyPassword(password, user.PasswordHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
