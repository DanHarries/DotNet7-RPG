namespace DotNet_RPG.API.Models
{
	public class Weapon
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		public int Damage { get; set; }

		public Character? Character { get; set; }

		// EF knows this is a corresponding foreign key for Character Model above. 
		public int CharacterId { get; set; }


	}
}
