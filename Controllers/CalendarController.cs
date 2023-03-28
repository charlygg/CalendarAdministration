using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using Microsoft.Identity.Web;
using CalendarAdministration.Models.Calendar;
using CalendarAdministration.Services;

namespace CalendarAdministration.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly IProfileService _profileService;

        public CalendarController(ICalendarService calendarService, IProfileService profileService)
        {
            _calendarService = calendarService;
            _profileService = profileService;
        }

        [AuthorizeForScopes(Scopes = new string[] { "Calendars.Read", "Calendars.ReadWrite" })]
        public async Task<IActionResult> Index()
        {
            var events = await _calendarService.GetCalendarEvents();
            var me = await _profileService.GetProfileAsync();
            var listOfEvents = new List<CalendarIndexView>();
            var today = DateTime.UtcNow;

            foreach(var ev in events)
            {
                listOfEvents.Add(new CalendarIndexView()
                {
                    OrganizerEmail = ev.Organizer.EmailAddress.Address,
                    Id = ev.Id,
                    bodyPreview = ev.BodyPreview,
                    Subject = ev.Subject,
                    StartDate = DateTime.Parse(ev.Start.DateTime),
                    EndDate = DateTime.Parse(ev.End.DateTime),
                    EndDateTimeZone = ev.End.TimeZone,
                    StatusResponse = ev.ResponseStatus.Response.HasValue ? 
                        ev.ResponseStatus.Response.Value : ResponseType.None,
                });
            }

            ViewData["myEmail"] = me?.Mail.ToString();
            ViewData["myTimeZone"] = TimeZoneInfo.Local;
            ViewData["today"] = today.ToString();

            return View(listOfEvents);
        }

        [AuthorizeForScopes(Scopes = new string[] { "Calendars.Read", "Calendars.ReadWrite" })]
        public async Task<IActionResult> Detail(string id)
        {
            var eventDetail = await _calendarService.GetCalendarEvent(id);
            var me = await _profileService.GetProfileAsync();

            if(eventDetail != null)
            {
                // Parsing class to our custom class to the view
                var element = new CalendarDetailView()
                {
                    OrganizerEmail = eventDetail.Organizer.EmailAddress.Address,
                    Subject = eventDetail.Subject,
                    Id = eventDetail.Id,
                    Content = eventDetail.Body.Content,
                    IsAllDay = eventDetail.IsAllDay.HasValue ? eventDetail.IsAllDay.Value : false ,
                    Importance = eventDetail.Importance.HasValue ? eventDetail.Importance.Value.ToString() : "normal",
                    StartDate = DateTime.Parse(eventDetail.Start.DateTime),
                    StartDateTimeZone = eventDetail.Start.TimeZone,
                    EndDate = DateTime.Parse(eventDetail.End.DateTime),
                    EndDateTimeZone = eventDetail.End.TimeZone
                };

                element.MyEmail = me?.Mail.ToString();

                // Fill attendees if the result have it
                var attendees = new List<Attendees>();

                eventDetail.Attendees.ToList().ForEach(attende =>
                {
                    attendees.Add(new Attendees()
                    {
                        Email = attende.EmailAddress.Address,
                        Name = attende.EmailAddress.Name,
                        StatusResponse = attende.Status.Response.HasValue ? attende.Status.Response.Value : ResponseType.None
                    });
                });

                element.Attendees = attendees;

                // Verify if I am the organizer to skip confirmation
                if(eventDetail.Organizer.EmailAddress.Address.Equals(me?.Mail.ToString()))
                {
                    element.AmIOrganizer = true;
                } else
                {
                    element.AmIOrganizer = false;
                }

                // Verify my response status for this meeting to display in the page
                element.MyStatusResponse = eventDetail.ResponseStatus.Response.HasValue ? 
                    eventDetail.ResponseStatus.Response.Value : ResponseType.None;
         
                return View(element);

            } else
            {
                return RedirectToAction(nameof(Index), "Calendar");
            }
        }

        [AuthorizeForScopes(Scopes = new string[] { "Calendars.Read", "Calendars.ReadWrite" })]
        [HttpPost]
        public async Task<IActionResult> Detail(CalendarDetailView model)
        {
            if (!ModelState.IsValid) return View(model);

            // Update request through Microsoft Graph 
            await _calendarService.UpdateResponseCalendarEvent(model.Id, model.ResponseComment, model.ResponseOptions);

            return RedirectToAction(nameof(Index), "Calendar");
        }
    }
}
