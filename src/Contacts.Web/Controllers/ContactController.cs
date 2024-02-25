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
            //Validations
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

            //Save new contact
            Contact newContact = contactVM.Contact;

            if (contactVM.CheckedGroups != null)
            {
                newContact.Groups = new List<Group>();

                foreach (var groupId in contactVM.CheckedGroups)
                {
                    newContact.Groups.Add(await _uof.GroupRepository.GetAsync(groupId));
                }
            }

            if (contactVM.InputWebsites != null)
            {
                newContact.Websites = new List<Website>();

                foreach (var url in contactVM.InputWebsites)
                {
                    newContact.Websites.Add(new Website
                    {
                        Url = url
                    });
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
                    Email = contact.Email,
                    Alias = contact.Alias,
                    Notes = contact.Notes,
                    Websites = contact.Websites,
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
                    Email = contact.Email,
                    Alias = contact.Alias,
                    Notes = contact.Notes,
                    Websites = contact.Websites
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
        public async Task<IActionResult> Edit(int id, ContactVM contactVM)
        {
            //Validations
            if (id != contactVM.Contact.Id)
            {
                return BadRequest();
            }

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

            //Save contact
            Contact contact2Edit = await _uof.ContactRepository.GetAsync(id);
            contact2Edit.Name = contactVM.Contact.Name;
            contact2Edit.Phone = contactVM.Contact.Phone;
            contact2Edit.Email = contactVM.Contact.Email;
            contact2Edit.Alias = contactVM.Contact.Alias;
            contact2Edit.Notes = contactVM.Contact.Notes;

            //Add groups to contact
            contact2Edit.Groups.Clear();

            if (contactVM.CheckedGroups != null)
            {
                contact2Edit.Groups = new List<Group>();

                foreach (var groupId in contactVM.CheckedGroups)
                {
                    contact2Edit.Groups.Add(await _uof.GroupRepository.GetAsync(groupId));
                }
            }

            //Add websites to contact
            contact2Edit.Websites.Clear();

            if (contactVM.InputWebsites != null)
            {
                contact2Edit.Websites = new List<Website>();

                foreach (var url in contactVM.InputWebsites)
                {
                    contact2Edit.Websites.Add(new Website
                    {
                        Url = url
                    });
                }
            }

            await _uof.ContactRepository.UpdateAsync(contact2Edit);
            int saveResult = await _uof.SaveAsync();

            if (saveResult > 0)
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
