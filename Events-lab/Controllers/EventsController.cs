using Events.data;
using Events_lab.Extensions;
using Events_lab.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Events_lab.Controllers
{
    [Authorize]
    public class EventsController : BaseController
    {
        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Events/My
        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var eventsDB = this.db.Events;
            var events = eventsDB
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {

                    var e = new Event()
                    {

                        Title = model.Title,
                        StartDateTime = model.StartDateTime,
                        Duration = model.Duration,
                        AuthorId = this.User.Identity.GetUserId(),
                        Description = model.Description,
                        Location = model.Location,
                        IsPublic = model.IsPublic
                    };
                    this.db.Events.Add(e);
                    this.db.SaveChanges();
                    this.AddNotification("Event created.", NotificationType.INFO);
                    // Display notification message "Event created."
                    return this.RedirectToAction("My");
            }
            return this.View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var eventToEdit = this.LoadEvent(id);
            if(eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id,
                    NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            /*
            var model = UpcomingPassedEventsViewModel.CreateFromEvent(eventToEdit);
            return this.View(model); */
            
            
            var model = this.db.Events.Where(s => s.Id == id).FirstOrDefault();
            return View(model); 
            /*
            Event even = db.Events.Find(id);
            return View();*/
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventInputModel model)
        {
            var eventToEdit = this.LoadEvent(id);
            if(eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id,
                    NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            if(model != null && this.ModelState.IsValid)
            {
                eventToEdit.Title = model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Description = model.Description;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                this.db.SaveChanges();
                this.AddNotification("Event edited", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        private Event LoadEvent(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToEdit = this.db.Events
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return eventToEdit;
        }
    }
}