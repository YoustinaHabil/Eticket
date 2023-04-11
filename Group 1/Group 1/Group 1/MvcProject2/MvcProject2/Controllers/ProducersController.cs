using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject2.Models;
using MvcProject2.Repository;

namespace MvcProject2.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ProducersController : Controller
    {
        private readonly IProducersRepository producersRepository;

        public ProducersController(IProducersRepository producersRepository)
        {
           this. producersRepository = producersRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allProducers = await producersRepository.GetAllAsync();
            return View(allProducers);
        }

        //GET: producers/details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await producersRepository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        //GET: producers/create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FullName,Bio")] Producer p ,IFormFile image)
        {
            //actor.ProfilePictureURL != null &
            if (p.FullName != null & p.Bio != null)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                  p.ProfilePictureURL = "/Images/" + fileName;
                }

                producersRepository.Add(p);
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }


        //GET: producers/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await producersRepository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public  IActionResult Edit(int id, [Bind("Id,FullName,Bio")] Producer p, IFormFile image)
        {

            if (p.FullName != null & p.Bio != null)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                  p.ProfilePictureURL = "/Images/" + fileName;
                }

                producersRepository.Update(id, p);
                return RedirectToAction(nameof(Index));
            }
            return View(p);
        }

        //GET: producers/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var producerDetails = await producersRepository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await producersRepository.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");

             producersRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
