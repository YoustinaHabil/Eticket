using MvcProject2.ViewModels;
using MvcProject2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcProject2.Repository;

namespace MvcProject2.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        IMovieRepository moviesRepository;

        public MoviesController(IMovieRepository _movieRepo)
        {
            moviesRepository = _movieRepo;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var allMovies = moviesRepository.GetAllAsync();
            return View(allMovies);
        }

        [AllowAnonymous]
        public IActionResult Filter(string searchString)
        {
            var allMovies = moviesRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allMovies.Where(n => string.Equals(n.Name, searchString, System.StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, System.StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResultNew);
            }

            return View("Index", allMovies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await moviesRepository.GetMovieById(id);
       
            return View(movieDetail);
        }

        public IActionResult Create()
        {
            var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
            var enumData = from MovieCategory e in Enum.GetValues(typeof(MovieCategory))
                           select new
                           {
                               Id = (int)e,
                               FullName = e.ToString()
                           };
            ViewBag.Categories = new SelectList(enumData, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewMovieVM movie, IFormFile image)
        {
            movie.ImageURL = "/Images/pexels - alex - azabache - 3879071.jpg";
            if (movie.Name != null && movie.Description != null && movie.Price != null
                 && movie.StartDate != null && movie.EndDate != null && movie.MovieCategory != null
                 && movie.ActorIds != null && movie.ActorIds.Count > 0 && movie.CinemaId != null
                 && movie.ProducerId != null)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    movie.ImageURL = "/Images/" + fileName;
                }

               await moviesRepository.AddNewMovieAsync(movie);
                return RedirectToAction("Index");
            }
            else
            {
                var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                var enumData = from MovieCategory e in Enum.GetValues(typeof(MovieCategory))
                               select new
                               {
                                   Id = (int)e,
                                   FullName = e.ToString()
                               };
                ViewBag.Categories = new SelectList(enumData, "Id", "FullName");

                return View(movie);
            }
        }


        public async Task<ActionResult> Edit(int id)
        {
            var movieDetails = await moviesRepository.GetMovieById(id);
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                ImageURL = movieDetails.ImageURL,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = (int)movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
            };

            var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
            var enumData = from MovieCategory e in Enum.GetValues(typeof(MovieCategory))
                           select new
                           {
                               Id = (int)e,
                               FullName = e.ToString()
                           };
            ViewBag.Categories = new SelectList(enumData, "Id", "FullName");

            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie, IFormFile image)
        {
            if (id != movie.Id) return View("NotFound");

            if (movie.Name != null && movie.Description != null && movie.Price != null
                && movie.StartDate != null && movie.EndDate != null && movie.MovieCategory != null
                && movie.ActorIds != null && movie.ActorIds.Count > 0 && movie.CinemaId != null
                && movie.ProducerId != null)
            {

                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    movie.ImageURL = "/Images/" + fileName;

                    await moviesRepository.UpdateMovieAsync(movie);

                    return RedirectToAction("Index");
                }
                await moviesRepository.UpdateMovieAsync(movie);

                return RedirectToAction("Index");
                //return View(movie);

            }
            var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
            var enumData = from MovieCategory e in Enum.GetValues(typeof(MovieCategory))
                           select new
                           {
                               Id = (int)e,
                               FullName = e.ToString()
                           };
            ViewBag.Categories = new SelectList(enumData, "Id", "FullName");

            return View(movie);


        }
        public IActionResult Delete(int id)
        {
            moviesRepository.DeleteMovie(id);
            return RedirectToAction("Index");
        }

    }
}