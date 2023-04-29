using DotNet_RPG.API.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _auth;

		public AuthController(IAuthRepository auth)
		{
			_auth = auth;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO request)
		{
			var res = await _auth.Register(
				new User { Username = request.Username }, request.Password
			);

			if (!res.Success)
			{
				return BadRequest(res);
			}

			return Ok(res);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(UserRegisterDTO request)
		{
			var res = await _auth.Login(request.Username, request.Password);

			if (!res.Success)
			{
				return BadRequest(res);
			}


			return Ok(res);
		}
	}
}
