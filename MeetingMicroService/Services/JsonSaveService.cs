
using Newtonsoft.Json;

namespace MeetingMicroService.Services
{
    public class JsonSaveService
    {
        private readonly string _directory;
        private readonly string _microservicePath = "AuthMicroservice";
        public JsonSaveService()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _microservicePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _directory = directory;
        }

        public void Save<T>(T data, string? filename = null) where T : class
        {
            if (string.IsNullOrEmpty(filename))
                filename = typeof(T).ToString();
           string completeFilePath = Path.Combine(_directory, filename);
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(completeFilePath, json);
                Console.WriteLine("Данные успешно сохранены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        public T Load<T>(string? filename = null) where T : class, new()
        {
            if (string.IsNullOrEmpty(filename))
                filename = typeof(T).ToString();
            string completeFilePath = Path.Combine(_directory, filename);
            if (File.Exists(completeFilePath))
            {
                try
                {
                    string json = File.ReadAllText(completeFilePath);
                    return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException("Ошибка загрузки данных");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Файл с данными не найден, создаем новый.");
            }

            return new T(); 
        }
    }
}
