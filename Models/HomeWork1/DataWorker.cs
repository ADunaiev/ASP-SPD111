using System.Diagnostics.Metrics;
using ASP_SPD111.Models.HomeWork1;
using ASP_SPD111.Data;

namespace ASP_SPD111.Models.HomeWork1
{
    public static class DataWorker
    {
        public static Restoraunt GetOneRestoraunt()
        {
            Restoraunt restoraunt = new ()
            {
                Name = "Kyiv",
                Address = "Maidan Nezalezhnosti, 1",
                KitchenType = "Ukrainian",
                Rating = 5
            };

            return restoraunt;
        }

        public static List<Restoraunt> GetAllRestoraunts()
        {
            List<Restoraunt> restoraunts = new List<Restoraunt>();

            restoraunts.Add(new Restoraunt()
            {
                Name = "Kyiv",
                Address = "Maidan Nezalezhnosti, 1",
                KitchenType = "Ukrainian",
                Rating = 5
            });

            restoraunts.Add(new Restoraunt()
            {
                Name = "More",
                Address = "T.Schevchenko prospect, 1",
                KitchenType = "Ukrainian",
                Rating = 5
            });

            restoraunts.Add(new Restoraunt()
            {
                Name = "Yakudza",
                Address = "Moldovanka",
                KitchenType = "Japanese",
                Rating = 4.5
            });

            return restoraunts;
        }

        public static List<Country> GetAllCountries()
        {
            List<Country> countryList = new List<Country>();

            countryList.Add(new Country()
            {
                Name = "Austalia", Capital = "Canberra", Population = 27.25, Domain = ".au"
            });
            countryList.Add(new Country()
            {
                Name = "Cambodia",
                Capital = "Phnom Penh",
                Population = 15.3,
                Domain = ".kh"
            });
            countryList.Add(new Country()
            {
                Name = "Djibouti",
                Capital = "Djibouti",
                Population = 0.92,
                Domain = ".dj"
            });
            countryList.Add(new Country()
            {
                Name = "East Timor",
                Capital = "Dili",
                Population = 1.066,
                Domain = ".tl"
            });
            countryList.Add(new Country()
            {
                Name = "Kiribati",
                Capital = "South Tarawa",
                Population = 0.104,
                Domain = ".ki"
            });
            return countryList;
        }



    }
}



