using DotNet_RPG.API.Dtos.Battle;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class BattleController : ControllerBase
	{
		private readonly IBattleService _battle;

		public BattleController(IBattleService battle)
		{
			_battle = battle;
		}


		[HttpPost("Weapon")]
		public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> WeaponAttack(WeaponAttackDTO request)
		{
			return Ok(await _battle.WeaponAttack(request));
		}

		[HttpPost("Skill")]
		public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> SkillAttack(SkillAttackDTO request)
		{
			return Ok(await _battle.SkillAttack(request));
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<FightResultDTO>>> Fight(FightRequestDTO request)
		{
			return Ok(await _battle.Fight(request));
		}
	}
}
