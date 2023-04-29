namespace DotNet_RPG.API.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{

		private readonly List<Character> characters = new()
		{
			new Character(),
			new Character {Id = 1, Name = "Sam"}
		};
		private readonly IMapper _mapper;

		public CharacterService(IMapper mapper)
		{
			_mapper = mapper;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO character)
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

			characters.Add(_mapper.Map<Character>(character));

			serviceResponse.Data = _mapper.Map<List<GetCharacterDTO>>(characters);

			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
		{
			var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>
			{
				Data = _mapper.Map<List<GetCharacterDTO>>(characters)
			};

			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id)
		{
			var serviceResponse = new ServiceResponse<GetCharacterDTO>();
			var character = characters.FirstOrDefault(x => x.Id == id);
			serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);

			return serviceResponse;


		}
	}
}
