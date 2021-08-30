using DataTable.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataTable.Controllers
{
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Address Address { get; set; }

        public AddressesController(ApplicationDbContext db)
        {
            _db = db;
        }

        #region cshtml hozzá meghívás

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminSite()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Address = new Address();
            if (id == null)
            {
                //Create
                return View(Address);
            }
            //Update
            Address = _db.Addresses.FirstOrDefault(u => u.AddressId == id);
            if (Address == null)
            {
                return NotFound();
            }
            return View(Address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Address.AddressId == 0)
                {
                    //Create
                    _db.Addresses.Add(Address);
                }
                else
                {
                    _db.Addresses.Update(Address);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Address);
        }

        #endregion cshtml hozzá meghívás

        #region Adatbázis json ként vissza adása

        /// <summary>
        /// Csak az aktív adatsorok kiolvasása
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            return Json(new { data = await _db.Addresses.Where(f => f.Active == 0).ToListAsync() });
        }

        /// <summary>
        /// Teljes adathalmaz kiolvasása
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Addresses.ToListAsync() });
        }

        #endregion Adatbázis json ként vissza adása

        #region Adattörlés

        /// <summary>
        /// Teljes törlés helye. Adatbázis kulcsok miatt nem engedi törölni az adatsorokat.
        /// Hogy a CascadeDelete megvalositható legyen modellezni kellene a többi táblát is amivel kapcsolatban van.
        /// </summary>
        [HttpDelete]
        public IActionResult CascadeDelete(int id)
        {
            try
            {
                var addressFromDb = _db.Addresses.SingleOrDefault(u => u.AddressId == id);
                if (addressFromDb == null)
                {
                    return Json(new { success = false, message = "Error while Deleting" });
                }

                _db.Addresses.Remove(addressFromDb);
                _db.SaveChanges();
                return Json(new { success = true, message = "Delete successfull" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Error while Deleting" });
            }
        }

        /// <summary>
        /// Inaktivvá teszi az adott rekordot.
        /// </summary>
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Address = _db.Addresses.FirstOrDefault(u => u.AddressId == id);
                if (Address == null)
                {
                    return Json(new { success = false, message = "Error while Deleting" });
                }
                Address.Active = 1;
                _db.SaveChanges();
                return Json(new { success = true, message = "Delete successfull" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
        }

        #endregion Adattörlés
    }
}