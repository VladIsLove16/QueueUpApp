using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharedModels.Models
{
    public class Meeting
    {
        public string GUID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<string> Members { get; set; } = new List<string>();
        public Meeting(string GUID,string title,DateTime date,string creator)
        {
            this.GUID = GUID; 
            Title = title; 
            Date = date;
            Organizer = creator;
        }
    }
}
