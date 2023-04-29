using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DotNet_RPG.API.Data
{
	public class AuthRepository : IAuthRepository
	{
		private readonly DataContext _context;
		private readonly IConfiguration _config;

		public AuthRepository(DataContext context, IConfiguration config)
		{
			_context = context;
			_config = config;
		}


		public async Task<ServiceResponse<string>> Login(string username, string password)
		{
			var res = new ServiceResponse<string>();
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

			if (user is null)
			{
				res.Success = false;
				res.Message = "User not found.";
			}

			else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			{
				res.Success = false;
				res.Message = "Wrong password";
			}
			else
			{
				res.Data = CreateToken(user);
			}

			return res;

		}

		public async Task<ServiceResponse<int>> Register(User user, string password)
		{
			var res = new ServiceResponse<int>();
			if (await UserExists(user.Username))
			{
				res.Success = false;
				res.Message = "User already exists";
				return res;
			}

			CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			res.Message = "User successfully created";
			res.Data = user.Id;

			return res;
		}

		public async Task<bool> UserExists(string username)
		{
			return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());

		}


		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new System.Security.Cryptography.HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}

		private string CreateToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Username)
			};

			var appSettingsToken = _config.GetSection("AppSettings:Token").Value;

			if (appSettingsToken is null)
			{
				throw new Exception("AppSettings Token is null!");
			}

			SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

			SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

			// Create final token
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			// Need JWT security handler
			JwtSecurityTokenHandler handler = new();
			SecurityToken token = handler.CreateToken(tokenDescriptor);

			// Serialize into JWT
			return handler.WriteToken(token);

		}
	}
}
