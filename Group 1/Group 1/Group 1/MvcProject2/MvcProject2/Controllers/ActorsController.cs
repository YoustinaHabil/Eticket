//using .Data;
//using eTickets.Data.Static;
using MvcProject2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcProject2.Repository;

namespace MvcProject2.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsController : Controller
    {
        IActorReposatory actorReposatory;   

        public ActorsController(IActorReposatory _actorRes)
        {
            actorReposatory = _actorRes;

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var allActors = actorReposatory.GetAll();
            return View(allActors);
        }

        //Get: Actors/Create\
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FullName,Bio")] Actor actor, IFormFile image)
        {
            //actor.ProfilePictureURL != null &
            if (actor.FullName != null & actor.Bio != null )
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    actor.ProfilePictureURL = "/Images/" + fileName;
                }

                actorReposatory.Insert(actor);
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }




        //Get: Actors/Details/1
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);

            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        //Get: Actors/Edit/1
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FullName,Bio")] Actor actor, IFormFile image)
        {
            if (actor.FullName != null & actor.Bio != null)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    actor.ProfilePictureURL = "/Images/" + fileName;
                }

                actorReposatory.Edit(id, actor);
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }





        //Get: Actors/Delete/1
        public IActionResult Delete(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);
            if (actorDetails == null) return View("NotFound");

            actorReposatory.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
