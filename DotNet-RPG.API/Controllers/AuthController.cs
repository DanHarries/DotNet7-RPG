using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
