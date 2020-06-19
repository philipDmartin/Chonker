using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chonker.Models;
using Chonker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Chonker.Controllers
{
    public class DogsController : Controller
    {
        private readonly DogRepository _dogRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogsController(IConfiguration config)
        {
            _dogRepo = new DogRepository(config);
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: Dog
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: Dog/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: Dog/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();

                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: Dog/Edit/5
        // GET: Dogs/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            int currentOwnerId = GetCurrentUserId();
            Dog dog = _dogRepo.GetDogById(id);
            if (dog == null)
            {
                return NotFound();
            }
            if (currentOwnerId != dog.OwnerId)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: Dog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);

            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: Dog/Delete/5
        public ActionResult Delete(int id)
        {
            int currentOwnerId = GetCurrentUserId();
            Dog dog = _dogRepo.GetDogById(id);

            if (currentOwnerId != dog.OwnerId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }
    }
}
