using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BattleController : ControllerBase
	{
		private readonly IBattleService _battle;

		public BattleController(IBattleService battle)
		{
			_battle = battle;
		}
	}
}
