using Microsoft.AspNetCore.Mvc;

using Contacts.DataAccess.Repository.Contracts;
using Contacts.Models;
using System.Security.Cryptography.Xml;

namespace Contacts.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IUnitOfWork _uow;

        public GroupController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.Id = string.IsNullOrEmpty(sort) ? "id_desc" : string.Empty;
            ViewBag.Name = sort == "name" ? "name_desc" : "name";

            var groups = _uow.GroupRepository.GetAllIQueryableAsync();

            switch (sort)
            {
                case "id_desc":
                    groups = groups.OrderByDescending(x => x.Id);
                    break;
                case "name":
                    groups = groups.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    groups = groups.OrderByDescending(x => x.Name);
                    break;
                default:
                    groups = groups.OrderBy(x => x.Id);
                    break;
            }

            return View(groups.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Group group)
        {
            if (!ModelState.IsValid)
            {
                return View(group);
            }

            await _uow.GroupRepository.AddAsync(group);
            int saveResult = await _uow.SaveAsync();

            if (saveResult > 0)
            {
                TempData["success"] = "Grupo creado con exito";
            }
            else
            {
                TempData["error"] = "Error al crear nuevo grupo";
            }

            return RedirectToAction("Index", "Group");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _uow.GroupRepository.GetAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            await _uow.GroupRepository.DeleteAsync(group);
            int deleteResult = await _uow.SaveAsync();

            if (deleteResult > 0)
            {
                TempData["success"] = "Grupo borrado con exito";
            }
            else
            {
                TempData["error"] = "Error al borrar grupo";
            }

            return RedirectToAction("Index", "Group");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _uow.GroupRepository.GetAsync((int)id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [FromForm] Group group)
        {
            if (Id != group.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(group);
            }

            await _uow.GroupRepository.UpdateAsync(group);
            int editResult = await _uow.SaveAsync();

            if (editResult > 0)
            {
                TempData["success"] = "Grupo editado con exito";
            }
            else
            {
                TempData["error"] = "Error al editar grupo";
            }

            return RedirectToAction("Index", "Group");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _uow.GroupRepository.GetAsync((int)id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

    }
}
