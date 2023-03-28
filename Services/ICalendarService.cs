using Microsoft.Graph;
using CalendarAdministration.Models.Calendar;

namespace CalendarAdministration.Services
{
    public interface ICalendarService   
    {
        public Task<IUserCalendarsCollectionPage> GetCalendars();
        public Task<IUserEventsCollectionPage> GetCalendarEvents();
        public Task<Event> GetCalendarEvent(string id);
        public Task UpdateResponseCalendarEvent(string id, string comment, ResponseEvent responseOptions);
    }
}
