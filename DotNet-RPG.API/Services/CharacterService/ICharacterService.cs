
namespace DotNet_RPG.API.Services.CharacterService
{
	public interface ICharacterService
	{
		Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters();
		Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id);
		Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO character);
	}
}
