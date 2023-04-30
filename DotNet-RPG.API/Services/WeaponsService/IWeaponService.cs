using DotNet_RPG.API.Dtos.Weapon;

namespace DotNet_RPG.API.Services.WeaponsService
{
	public interface IWeaponService
	{
		Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO newWeapon);
	}
}
