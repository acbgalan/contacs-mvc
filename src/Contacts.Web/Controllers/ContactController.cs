using Contacts.DataAccess.Repository.Contracts;
using Contacts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _uof;

        public ContactController(IUnitOfWork unitOfWork)
        {
            _uof = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _uof.ContactRepository.GetAllAsync();

            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _uof.ContactRepository.AddAsync(contact);
            int saveResult = await _uof.SaveAsync();

            if (saveResult > 0)
            {
                TempData["success"] = "Contacto creado con exito";
            }
            else
            {
                TempData["error"] = "Error al crear contacto";
            }

            return RedirectToAction("Index", "Contact");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _uof.ContactRepository.GetAsync((int)id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Contact contact)
        {
            if(id != contact.Id)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return View(contact);
            }

            await _uof.ContactRepository.UpdateAsync(contact);
            int editResult = await _uof.SaveAsync();

            if (editResult > 0)
            {
                TempData["success"] = "Contacto editado con exito";
            }
            else
            {
                TempData["error"] = "Error al editar contacto";
            }

            return RedirectToAction("Index", "Contact");
        }


    }
}
