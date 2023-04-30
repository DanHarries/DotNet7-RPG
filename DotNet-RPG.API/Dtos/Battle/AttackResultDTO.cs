namespace DotNet_RPG.API.Dtos.Battle
{
	public class AttackResultDTO
	{
		public string Attacker { get; set; } = string.Empty;
		public string Opponent { get; set; } = string.Empty;

		public int AttackerHP { get; set; }
		public int OpponentHP { get; set; }
		public int Damage { get; set; }
	}
}
