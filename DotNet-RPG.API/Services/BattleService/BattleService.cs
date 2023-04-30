using DotNet_RPG.API.Dtos.Battle;

namespace DotNet_RPG.API.Services.BattleService
{
	public class BattleService : IBattleService
	{
		private readonly DataContext _context;

		public BattleService(DataContext context)
		{
			_context = context;
		}


		public async Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO attack)
		{
			var res = new ServiceResponse<AttackResultDTO>();

			try
			{
				var attacker = await _context.Characters
					.Include(c => c.Weapon)
					.FirstOrDefaultAsync(c => c.Id == attack.AttackerId);

				var opponent = await _context.Characters
					.Include(c => c.Weapon)
					.FirstOrDefaultAsync(c => c.Id == attack.OpponentId);

				if (attacker is null || opponent is null || attacker.Weapon is null)
				{
					throw new Exception("Something fishy is going on ere mayyte!");
				}

				int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
				damage -= new Random().Next(opponent.Defeats);

				if (damage > 0)
				{
					opponent.HitPoints -= damage;
				}

				if (opponent.HitPoints <= 0)
				{
					opponent.HitPoints = 0;
					res.Message = $"{opponent.Name} has been defeated!";
				}

				await _context.SaveChangesAsync();

				res.Data = new AttackResultDTO
				{
					Attacker = attacker.Name,
					Opponent = opponent.Name,
					AttackerHP = attacker.HitPoints,
					OpponentHP = opponent.HitPoints,
					Damage = damage
				};

			}
			catch (Exception ex)
			{
				res.Success = false;
				res.Message = ex.Message;

				return res;
			}

			return res;
		}
		public async Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO req)
		{
			var res = new ServiceResponse<AttackResultDTO>();

			try
			{
				var attacker = await _context.Characters
					.Include(c => c.Skills)
					.FirstOrDefaultAsync(c => c.Id == req.AttackerId);

				var opponent = await _context.Characters
					.Include(c => c.Skills)
					.FirstOrDefaultAsync(c => c.Id == req.OpponentId);

				if (attacker is null || opponent is null || attacker.Skills is null)
				{
					throw new Exception("Something fishy is going on ere mayyte!");
				}

				var skill = attacker.Skills.FirstOrDefault(s => s.Id == req.SkillId);

				if (skill is null)
				{
					res.Success = false;
					res.Message = $"{attacker.Name} doesn't know that skill!";
					return res;
				}

				int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
				damage -= new Random().Next(opponent.Defeats);

				if (damage > 0)
				{
					opponent.HitPoints -= damage;
				}

				if (opponent.HitPoints <= 0)
				{
					opponent.HitPoints = 0;
					res.Message = $"{opponent.Name} has been defeated!";
				}

				await _context.SaveChangesAsync();

				res.Data = new AttackResultDTO
				{
					Attacker = attacker.Name,
					Opponent = opponent.Name,
					AttackerHP = attacker.HitPoints,
					OpponentHP = opponent.HitPoints,
					Damage = damage
				};

			}
			catch (Exception ex)
			{
				res.Success = false;
				res.Message = ex.Message;

				return res;
			}

			return res;
		}
	}
}
