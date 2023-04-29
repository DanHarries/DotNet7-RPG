namespace DotNet_RPG.API.Data
{
	public class AuthRepository : IAuthRepository
	{
		private readonly DataContext _context;

		public AuthRepository(DataContext context)
		{
			_context = context;
		}


		public Task<ServiceResponse<string>> Login(string username, string password)
		{
			throw new NotImplementedException();
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
	}
}
