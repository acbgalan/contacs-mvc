using Microsoft.AspNetCore.Mvc;

using Contacts.DataAccess.Repository.Contracts;
using Contacts.Models;

namespace Contacts.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IUnitOfWork _uow;

        public GroupController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var groups = await _uow.GroupRepository.GetAllAsync();

            return View(groups);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group group)
        {
            if(!ModelState.IsValid)
            {
                return View(group);
            }

            await _uow.GroupRepository.AddAsync(group);
            int saveResult = await _uow.GroupRepository.SaveAsync();

            if(saveResult > 0)
            {
                TempData["success"] = "Grupo creado con exito";
            } else
            {
                TempData["error"] = "Error al crear nuevo grupo";
            }

            return RedirectToAction("Index", "Group");
        }


    }
}
