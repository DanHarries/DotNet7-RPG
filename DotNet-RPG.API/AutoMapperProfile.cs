﻿using DotNet_RPG.API.Dtos.Battle;
using DotNet_RPG.API.Dtos.Skill;
using DotNet_RPG.API.Dtos.Weapon;

namespace DotNet_RPG.API
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Character, GetCharacterDTO>();
			CreateMap<AddCharacterDTO, Character>();
			CreateMap<Weapon, GetWeaponDTO>();
			CreateMap<Skill, GetSkillDTO>();
			CreateMap<Character, HighScoreDTO>();
		}


	}
}
