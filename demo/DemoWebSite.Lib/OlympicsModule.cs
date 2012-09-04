using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoWebSite
{
	public class OlympicsModule : IOlympicsModule
	{
		public List<Country> GetCountries()
		{
			var countries = new List<Country>();

			countries.Add(new Country("USA", "Unites States", new Medals() { Gold = 46, Silver = 29, Bronze = 29 }));
			countries.Add(new Country("CHN", "China", new Medals() { Gold = 38, Silver = 27, Bronze = 23 }));
			countries.Add(new Country("GBR", "Great Britain", new Medals() { Gold = 29, Silver = 17, Bronze = 19 }));
			countries.Add(new Country("RUS", "Russia", new Medals() { Gold = 24, Silver = 26, Bronze = 32 }));
			countries.Add(new Country("KOR", "Korea", new Medals() { Gold = 13, Silver = 8, Bronze = 7 }));

			return countries;
		}

		public string GetCountryName(string code)
		{
			return GetCountries().First(p => p.Code == code.ToUpper()).Name;
		}

		public List<Athlete> GetAthletes(string country)
		{
			var athletes = new List<Athlete>();

			if (country == "usa")
			{
				athletes.Add(new Athlete("CHANDLER", "Tyson", 29, "New York Knicks"));
				athletes.Add(new Athlete("DURANT", "Kevin", 23, "Oklahoma City Thunder"));
				athletes.Add(new Athlete("JAMES", "Lebron", 27, "Miami Heat"));
				athletes.Add(new Athlete("WESTBROOK", "Russell", 23, "Oklahoma City Thunder"));
				athletes.Add(new Athlete("WILLIAMS", "Deron", 28, "Brooklyn Nets"));
				athletes.Add(new Athlete("IGUODALA", "Andre", 28, "Philadelphia 76ers"));
				athletes.Add(new Athlete("BRYANT", "Kobe", 34, "Los Angeles Lakers"));
				athletes.Add(new Athlete("LOVE", "Kevin", 23, "Minnesota Timberwolves"));
				athletes.Add(new Athlete("HARDEN", "James", 23, "Oklahoma City Thunder"));
				athletes.Add(new Athlete("PAUL", "Chris", 27, "Los Angeles Clippers"));
				athletes.Add(new Athlete("DAVIS", "Anthony", 19, "New Orleans Hornets"));
				athletes.Add(new Athlete("ANTHONY", "Carmelo", 28, "New York Knicks"));
			}
			else if (country == "chn")
			{
				athletes.Add(new Athlete("DING", "Jinhui", 22, "Zhejiang"));
				athletes.Add(new Athlete("LIU", "Wei", 32, "Shanghai"));
				athletes.Add(new Athlete("YI", "Li", 24, "Jiangsu"));
				athletes.Add(new Athlete("WANG", "Shipeng", 29, "Guangdong"));
				athletes.Add(new Athlete("ZHU", "Fangyu", 29, "Guangdong"));
				athletes.Add(new Athlete("SUN", "Yue", 26, "Aoshen"));
			}

			return athletes;
		}
	}
}
