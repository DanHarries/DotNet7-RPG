using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CharacterController : Controller
	{
		private readonly List<Character> characters = new()
		{
			new Character(),
			new Character {Id = 1, Name = "Sam"}
		};

		[HttpGet("GetAll")]
		public ActionResult<List<Character>> Get()
		{

			return Ok(characters);

		}

		[HttpGet("{id}")]
		public ActionResult<Character> GetSingle(int id)
		{

			return Ok(characters.FirstOrDefault(x => x.Id == id));

		}
	}

}
