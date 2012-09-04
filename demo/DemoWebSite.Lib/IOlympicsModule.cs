using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoWebSite
{
	public interface IOlympicsModule
	{
		List<Country> GetCountries();

		string GetCountryName(string code);

		List<Athlete> GetAthletes(string country);
	}

	public class Medals
	{
		public int Gold { get; set; }
		public int Silver { get; set; }
		public int Bronze { get; set; }
	}

	public class Athlete
	{
		public Athlete(string lastName, string firstName, int age, string club)
		{
			FirstName = firstName;
			LastName = lastName;
			Age = age;
			Club = club;
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string Club { get; set; }
	}

	public class Country
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public Medals Medals { get; set; }

		public Country(string code, string name, Medals medals)
		{
			Code = code;
			Name = name;
			Medals = medals;
		}
	}
}
