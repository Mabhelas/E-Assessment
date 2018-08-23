using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Events.data;

namespace Events_lab.Models
{
    public class UpcomingPassedEventsViewModel
    {
        public IEnumerable<EventViewModel> UpcomingEvents { get; set; }

        public IEnumerable<EventViewModel> PassedEvents { get; set; }

        internal static UpcomingPassedEventsViewModel CreateFromEvent(UpcomingPassedEventsViewModel eventToEdit)
        {
            return eventToEdit;
        }
    }
}