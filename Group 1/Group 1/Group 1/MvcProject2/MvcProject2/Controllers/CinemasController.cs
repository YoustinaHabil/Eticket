
using MvcProject2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcProject2.Repository;

namespace MvcProject2.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CinemasController : Controller
    {
        ICinemasReposatory cinemasReposatory;

        public CinemasController(ICinemasReposatory _cinemaRes)
        {
            cinemasReposatory = _cinemaRes;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var allCinemas = cinemasReposatory.GetAll();
            return View(allCinemas);
        }


        //Get: Cinemas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description")] Cinema cinema, IFormFile image)
        {
            //actor.ProfilePictureURL != null &
            if (cinema.Name != null & cinema.Description != null)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    cinema.Logo = "/Images/" + fileName;
                }

                cinemasReposatory.Insert(cinema);
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        //Get: Cinemas/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = cinemasReposatory.GetByID(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        //Get: Cinemas/Edit/1
        public IActionResult Edit(int id)
        {
            var cinemaDetails = cinemasReposatory.GetByID(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description")] Cinema cinema , IFormFile image)
        {

            if (cinema.Name != null & cinema.Description != null)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    cinema.Logo = "/Images/" + fileName;
                }

                cinemasReposatory.Edit(id, cinema);
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }


        //Get: Cinemas/Delete/1
        public IActionResult Delete(int id)
        {
            var cinemaDetails = cinemasReposatory.GetByID(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            var cinemaDetails = cinemasReposatory.GetByID(id);
            if (cinemaDetails == null) return View("NotFound");

            cinemasReposatory.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
