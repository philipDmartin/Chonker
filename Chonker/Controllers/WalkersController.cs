using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chonker.Models;
using Chonker.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Chonker.Controllers
{
    public class WalkersController : Controller
    {
        private readonly WalkerRepository _walkerRepo;
        private readonly WalkRepository _walkRepo;
        private readonly OwnerRepository _ownerRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkersController(IConfiguration config)
        {
            _walkerRepo = new WalkerRepository(config);
            _walkRepo = new WalkRepository(config);
            _ownerRepo = new OwnerRepository(config);
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        //GET: Walkers
        public ActionResult Index()
        {
            try
            {
                int currentOwnerId = GetCurrentUserId();
                Owner currentOwner = _ownerRepo.GetOwnerById(currentOwnerId);
                int currentNeighborhoodId = currentOwner.NeighborhoodId;
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(currentNeighborhoodId);
                if (currentNeighborhoodId != currentOwner.NeighborhoodId)
                {
                    return NotFound();
                }
                return View(walkers);
            }
            catch
            {
                List<Walker> allWalkers = _walkerRepo.GetAllWalkers();
                return View(allWalkers);
            }
        }

        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return NotFound();
            }

            return View(walker);
        }

        // GET: Walkers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Walkers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Walkers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Walkers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Walkers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Walkers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
