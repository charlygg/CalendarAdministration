using Microsoft.Graph;

namespace CalendarAdministration.Models.Calendar
{
    public class CalendarIndexView
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string bodyPreview { get; set; }
        public string OrganizerEmail { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateTimeZone { get; set; }
        public DateTime? EndDate { get; set;}
        public string EndDateTimeZone { get; set; }
        public ResponseType StatusResponse { get; set; }
    }
}
