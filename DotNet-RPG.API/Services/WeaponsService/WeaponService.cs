using DotNet_RPG.API.Dtos.Weapon;
using System.Security.Claims;

namespace DotNet_RPG.API.Services.WeaponsService
{
	public class WeaponService : IWeaponService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO newWeapon)
		{
			var response = new ServiceResponse<GetCharacterDTO>();

			try
			{
				var character = await _context.Characters
					.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
					c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!));

				if (character is null)
				{
					response.Success = false;
					response.Message = "Character not found";
					return response;
				}

				// Could use AutoMap
				var weapon = new Weapon
				{
					Name = newWeapon.Name,
					Damage = newWeapon.Damage,
					Character = character
				};

				_context.Weapons.Add(weapon);

				await _context.SaveChangesAsync();

				response.Data = _mapper.Map<GetCharacterDTO>(character);

				return response;

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;

				return response;
			}
		}
	}
}
