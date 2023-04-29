﻿
namespace DotNet_RPG.API.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		public DbSet<Character> Characters { get; set; }



	}
}