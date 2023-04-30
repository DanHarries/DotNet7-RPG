
namespace DotNet_RPG.API.Services.CharacterService
{
	public interface ICharacterService
	{
		Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters();
		Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id);
		Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO addCharacter);

		Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacter);

		Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id);

		Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDTO newSkill);
	}
}
