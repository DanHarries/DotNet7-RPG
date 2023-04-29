namespace DotNet_RPG.API.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _db;

		public CharacterService(IMapper mapper, DataContext db)
		{
			_mapper = mapper;
			_db = db;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO addCharacter)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
			var character = _mapper.Map<Character>(addCharacter);

			_db.Characters.Add(character);
			await _db.SaveChangesAsync();

			serviceResponse.Data = await _db.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();

			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

			try
			{
				var character = await _db.Characters.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Character with Id '{id}' not found.");

				_db.Characters.Remove(character);

				await _db.SaveChangesAsync();

				serviceResponse.Data = await _db.Characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}

			return serviceResponse;
		}


		public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters(int userId)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

			var dbCharacters = await _db.Characters.Where(x => x.User!.Id == userId).ToListAsync();

			serviceResponse.Data = _mapper.Map<List<GetCharacterDTO>>(dbCharacters);

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDTO>();
			var dbCharacter = await _db.Characters.FirstOrDefaultAsync(c => c.Id == id);
			serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDTO>();

			try
			{
				var character = await _db.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id) ?? throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");

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
	}
}
