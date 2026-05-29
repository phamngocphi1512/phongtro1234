using Microsoft.AspNetCore.Mvc;

namespace QuanLiPhongTro.Controllers
{
    public class UserPhongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}