namespace DotNet_RPG.API.Data
{
	public class AuthRepository : IAuthRepository
	{
		private readonly DataContext _context;

		public AuthRepository(DataContext context)
		{
			_context = context;
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
				res.Data = user.Id.ToString();
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
	}
}
