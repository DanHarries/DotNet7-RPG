﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNet_RPG.API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CharacterController : ControllerBase
	{
		private readonly ICharacterService _characterService;

		public CharacterController(ICharacterService characterService)
		{
			_characterService = characterService;
		}

		[HttpGet("GetAll")]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get()
		{
			int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
			return Ok(await _characterService.GetAllCharacters(id));

		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id)
		{

			return Ok(await _characterService.GetCharacterById(id));

		}

		[HttpPost]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter(AddCharacterDTO character)
		{

			return Ok(await _characterService.AddCharacter(character));

		}

		[HttpPut]
		public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> UpdateCharacter(UpdateCharacterDTO updatedCharacter)
		{
			var res = await _characterService.UpdateCharacter(updatedCharacter);

			if (res.Data is null)
			{
				return NotFound(res);
			}

			return Ok(res);

		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> DeleteCharacter(int id)
		{

			var res = await _characterService.DeleteCharacter(id);

			if (res.Data is null)
			{
				return NotFound(res);
			}

			return Ok(res);

		}
	}

}
