
using Microsoft.Graph;
using CalendarAdministration.Models.Calendar;

namespace CalendarAdministration.Services
{
    public class CalendarService : ICalendarService
    {
        private GraphServiceClient _graphServiceClient;
        private ILogger<CalendarService> _logger;
        public CalendarService(GraphServiceClient graphServiceClient, ILogger<CalendarService> logger)
        {
            _graphServiceClient = graphServiceClient;
            _logger = logger;
        }

        public async Task<IUserEventsCollectionPage> GetCalendarEvents()
        {
            _logger.LogInformation("Calling GetCalendarEvents");

            var result = await _graphServiceClient.Me.Events.Request().GetAsync();
          
            return result;
        }

        public async Task<IUserCalendarsCollectionPage> GetCalendars()
        {
            _logger.LogInformation("Calling GetCalendars");

            var result = await _graphServiceClient.Me.Calendars.Request().GetAsync();

            return result;
        }

        public async Task<Event> GetCalendarEvent(string id)
        {
            _logger.LogInformation("Calling GetCalendarEvent");

            var result = await _graphServiceClient.Me.Events[id].Request().GetAsync();

            return result;
        }

        public async Task UpdateResponseCalendarEvent(string id, string comment, ResponseEvent responseOptions)
        {

            _logger.LogInformation("Calling UpdateResponseCalendarEvent");

            var responseBody = new
            {
                Comment = comment,
                SendResponse = true,
            };

            if (responseOptions == ResponseEvent.Accept)
            {
                await _graphServiceClient.Me.Events[id].Accept(responseBody.Comment, responseBody.SendResponse).Request().PostAsync();

            } else if (responseOptions == ResponseEvent.Decline)
            {
                await _graphServiceClient.Me.Events[id].Decline(responseBody.Comment, responseBody.SendResponse).Request().PostAsync();
            }
        }
    }
}
