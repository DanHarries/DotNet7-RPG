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

				int damage = DoWeaponAttack(attacker, opponent);

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

				int damage = DoSkillAttack(attacker, opponent, skill);

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



		public async Task<ServiceResponse<FightResultDTO>> Fight(FightRequestDTO req)
		{
			var response = new ServiceResponse<FightResultDTO>()
			{
				Data = new FightResultDTO()

			};

			try
			{
				var characters = await _context.Characters
					.Include(w => w.Weapon)
					.Include(s => s.Skills)
					.Where(x => req.CharacterIds.Contains(x.Id))
					.ToListAsync();

				bool defeated = false;

				while (!defeated)
				{
					foreach (var attacker in characters)
					{
						var opponents = characters.Where(x => x.Id != attacker.Id).ToList();
						var opponent = opponents[new Random().Next(opponents.Count)];

						int damage = 0;
						string attackUsed = string.Empty;

						bool useWeapon = new Random().Next(2) == 0;

						if (useWeapon && attacker.Weapon is not null)
						{
							attackUsed = attacker.Weapon.Name;
							damage = DoWeaponAttack(attacker, opponent);
						}
						else if (!useWeapon && attacker.Skills is not null)
						{
							var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
							attackUsed = skill.Name;
							damage = DoSkillAttack(attacker, opponent, skill);
						}
						else
						{
							response.Data.BattleLog.Add($"{attacker.Name} wasn't able to attack");
							continue;
						}

						response.Data.BattleLog.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

						if (opponent.HitPoints <= 0)
						{
							defeated = true;
							attacker.Victories++;
							opponent.Defeats++;
							response.Data.BattleLog.Add($"{opponent.Name} has been defeated");
							response.Data.BattleLog.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
							break;
						}

					}
				}

				characters.ForEach(x =>
				{
					x.Fights++;
					x.HitPoints = 100;
				});

				await _context.SaveChangesAsync();

				return response;

			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return response;
			}



		}

		private static int DoWeaponAttack(Character attacker, Character opponent)
		{
			if (attacker.Weapon is null)
			{
				throw new Exception("Attacker has no weapon!");
			}

			int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
			damage -= new Random().Next(opponent.Defeats);

			if (damage > 0)
			{
				opponent.HitPoints -= damage;
			}

			return damage;
		}

		private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
		{

			int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
			damage -= new Random().Next(opponent.Defeats);

			if (damage > 0)
			{
				opponent.HitPoints -= damage;
			}

			return damage;
		}
	}
}
