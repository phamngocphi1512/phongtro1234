using Microsoft.AspNetCore.Mvc;

namespace QuanLiPhongTro.Controllers
{
    public class UserTaiKhoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}