using Microsoft.AspNetCore.Mvc;

namespace NguyenDaiHiep_2180605809_week_three.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
