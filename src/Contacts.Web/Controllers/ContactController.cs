using Contacts.DataAccess.Repository.Contracts;
using Contacts.Models;
using Contacts.Models.ViewModels;
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

        public async Task<IActionResult> Create()
        {
            var groups = await _uof.GroupRepository.GetAllAsync();

            ContactVM contactVM = new ContactVM()
            {
                Contact = new Contact(),
                GroupCheckList = groups.OrderBy(x => x.Name).Select(x => new CheckboxVM
                {
                    Id = x.Id,
                    LabelName = x.Name,
                    IsChecked = false
                })
            };

            return View(contactVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactVM contactVM)
        {
            if (!ModelState.IsValid)
            {
                var groups = await _uof.GroupRepository.GetAllAsync();

                contactVM.GroupCheckList = groups.OrderBy(x => x.Name).Select(x => new CheckboxVM
                {
                    Id = x.Id,
                    LabelName = x.Name,
                    IsChecked = contactVM.CheckedGroups != null ? contactVM.CheckedGroups.Contains(x.Id) : false
                });

                return View(contactVM);
            }

            Contact newContact = contactVM.Contact;

            if (contactVM.CheckedGroups != null)
            {
                newContact.Groups = new List<Group>();

                foreach (var groupId in contactVM.CheckedGroups)
                {
                    newContact.Groups.Add(await _uof.GroupRepository.GetAsync(groupId));
                }
            }

            await _uof.ContactRepository.AddAsync(newContact);
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

        public async Task<IActionResult> Details(int? id)
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

            var groups = await _uof.GroupRepository.GetAllAsync();

            ContactVM contactVM = new ContactVM
            {
                Contact = new Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Phone = contact.Phone,
                    Email = contact.Email
                },
                GroupCheckList = groups.OrderBy(x => x.Name).Select(x => new CheckboxVM
                {
                    Id = x.Id,
                    LabelName = x.Name,
                    IsChecked = contact.Groups.Any(g => g.Id == x.Id)
                })
            };

            return View(contactVM);
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

            var groups = await _uof.GroupRepository.GetAllAsync();

            ContactVM contactVM = new ContactVM()
            {
                Contact = new Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Phone = contact.Phone,
                    Email = contact.Email
                },
                GroupCheckList = groups.OrderBy(x => x.Name).Select(x => new CheckboxVM
                {
                    Id = x.Id,
                    LabelName = x.Name,
                    IsChecked = contact.Groups.Any(g => g.Id == x.Id)
                })
            };

            return View(contactVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection form)
        {
            int formId;
            int.TryParse(form["Contact.Id"], out formId);

            if (id != formId)
            {
                return BadRequest();
            }

            var contact2Edit = await _uof.ContactRepository.GetAsync(id);

            if (contact2Edit == null)
            {
                return NotFound();
            }

            contact2Edit.Name = form["Contact.Name"];
            contact2Edit.Phone = form["Contact.Phone"];
            contact2Edit.Email = form["Contact.Email"];
            contact2Edit.Groups.Clear();

            foreach (var key in form.Keys)
            {
                if (key.StartsWith("grp-"))
                {
                    int groupId = int.Parse(form[key]);
                    contact2Edit.Groups.Add(await _uof.GroupRepository.GetAsync(groupId));
                }
            }

            await _uof.ContactRepository.UpdateAsync(contact2Edit);
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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _uof.ContactRepository.GetAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            await _uof.ContactRepository.DeleteAsync(contact);
            int deleteResult = await _uof.SaveAsync();

            if (deleteResult > 0)
            {
                TempData["success"] = "Contacto borrado con exito";
            }
            else
            {
                TempData["error"] = "Error al borrar contacto";
            }

            return RedirectToAction("Index", "Contact");
        }

    }
}
