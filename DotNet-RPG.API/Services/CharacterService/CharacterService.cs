using System.Security.Claims;

namespace DotNet_RPG.API.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _db;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CharacterService(IMapper mapper, DataContext db, IHttpContextAccessor httpContextAccessor)
		{
			_mapper = mapper;
			_db = db;
			_httpContextAccessor = httpContextAccessor;
		}


		public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO addCharacter)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
			var character = _mapper.Map<Character>(addCharacter);

			character.User = await _db.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

			_db.Characters.Add(character);
			await _db.SaveChangesAsync();

			serviceResponse.Data = await _db.Characters
				.Where(id => id.User!.Id == GetUserId())
				.Select(c => _mapper.Map<GetCharacterDTO>(c))
				.ToListAsync();

			return serviceResponse;
		}


		public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

			try
			{
				var character = await _db.Characters
					.Where(x => x.Id == id && x.User!.Id == GetUserId())
					.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Character with Id '{id}' not found.");

				_db.Characters.Remove(character);

				await _db.SaveChangesAsync();

				serviceResponse.Data = await _db.Characters
					.Where(x => x.User!.Id == GetUserId())
					.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}

			return serviceResponse;
		}


		public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
		{


			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

			var dbCharacters = await _db.Characters
				.Include(c => c.Weapon)
				.Include(c => c.Skills)
				.Where(x => x.User!.Id == GetUserId())
				.ToListAsync();

			serviceResponse.Data = _mapper.Map<List<GetCharacterDTO>>(dbCharacters);

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDTO>();
			var dbCharacter = await _db.Characters
				.Include(c => c.Weapon)
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
			serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);

			if (dbCharacter == null)
			{
				serviceResponse.Message = "No character found for this user";
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDTO>();

			try
			{
				// Add include to get related objects
				var character = await _db.Characters
					.Include(c => c.User)
					.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

				if (character is null || character.User!.Id != GetUserId())
				{
					throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");
				}

				character.Name = updatedCharacter.Name;
				character.HitPoints = updatedCharacter.HitPoints;
				character.Strength = updatedCharacter.Strength;
				character.Defense = updatedCharacter.Defense;
				character.Intelligence = updatedCharacter.Intelligence;
				character.Class = updatedCharacter.Class;

				await _db.SaveChangesAsync();

				serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDTO newSkill)
		{
			var res = new ServiceResponse<GetCharacterDTO>();

			try
			{
				var character = await _db.Characters
					.Include(c => c.Weapon)
					.Include(c => c.Skills)
					// Can use then include to include more ... 
					.FirstOrDefaultAsync(c => c.Id == newSkill.CharacterId &&
					c.User!.Id == GetUserId());

				if (character is null)
				{
					res.Success = false;
					res.Message = "Character not found";
					return res;
				}

				var skill = await _db.Skills
					.FirstOrDefaultAsync(s => s.Id == newSkill.SkillId);

				if (skill is null)
				{
					res.Success = false;
					res.Message = "Skill not found";
					return res;
				}

				character.Skills!.Add(skill);

				await _db.SaveChangesAsync();

				res.Data = _mapper.Map<GetCharacterDTO>(character);

				return res;
			}
			catch (Exception ex)
			{
				res.Success = false;
				res.Message = ex.Message;

				return res;

			}
		}



		//Private methods ... 
		private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
	}
}
