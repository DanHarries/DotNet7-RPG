using DotNet_RPG.API.Dtos.Weapon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class WeaponController : ControllerBase
	{
		private readonly IWeaponService _weaponService;

		public WeaponController(IWeaponService weaponService)
		{
			_weaponService = weaponService;
		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddWeapon(AddWeaponDTO newWeapon)
		{
			return Ok(await _weaponService.AddWeapon(newWeapon));
		}

	}
}
