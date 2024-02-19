using Contacts.DataAccess.Repository.Contracts;
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
    }
}
