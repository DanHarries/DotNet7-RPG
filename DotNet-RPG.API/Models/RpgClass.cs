using System.Text.Json.Serialization;

namespace DotNet_RPG.API.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum RpgClass
	{
		Knight = 1,
		Mage = 2,
		Healer = 3
	}
}
