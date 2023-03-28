using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Graph;

namespace CalendarAdministration.Models.Calendar
{
    public class CalendarDetailView
    {
        public string? Id { get; set; }
        public string? Subject { get; set; }
        public string? Importance { get; set; }
        public bool IsAllDay { get; set; }
        public string? Content { get; set; }
        public string? OrganizerEmail { get; set; }
        public ResponseEvent ResponseOptions { get; set; }
        public DateTime? StartDate { get; set; }
        public string? StartDateTimeZone { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EndDateTimeZone { get; set; }
        public List<Attendees> Attendees { get; set; }
        // Additional attributes for the view
        public string? ResponseComment { get; set; }
        public bool AmIOrganizer { get; set; } = false;
        public string? MyEmail { get; set; }
        public ResponseType MyStatusResponse { get; set; }

    }

    public class Attendees
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public ResponseType StatusResponse { get; set; }
        public DateTime TimeResponse { get; set; }
    }

    public enum ResponseEvent
    {
        Accept = 1,
        Decline = 2
    }
}
