﻿using DotNet_RPG.API.Dtos.Battle;

namespace DotNet_RPG.API.Services.BattleService
{
	public interface IBattleService
	{
		Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO attack);
		Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO attack);
		Task<ServiceResponse<FightResultDTO>> Fight(FightRequestDTO req);

		Task<ServiceResponse<List<HighScoreDTO>>> GetHighScore();

	}
}
