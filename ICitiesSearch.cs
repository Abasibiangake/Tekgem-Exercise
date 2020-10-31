using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {   
        //input search string
        string searchInp = "l";

        CitySearch.Finder finder = new CitySearch.Finder();

        /*.ToUpper is essential, because without it, if input string is writing in lower case, 
        it outputs an error. Therefore it has to always be converted to upper case to output required result. */
        CitySearch.ICityResult cities = finder.Search(searchInp.ToUpper());

        //output on terminal, all cities that start with the search string
        Console.WriteLine("\n All cities marching search:" + cities.NextCities.Count);
        foreach (string cityNames in  cities.NextCities) {
			Console.WriteLine("`" + cityNames + "`");
		}

        //output all valid next character for each matched city
		Console.WriteLine("\nNext Letters for each marching cities:" + cities.NextLetters.Count);
        foreach (string cityletter in  cities.NextLetters) {
			Console.WriteLine("`" + cityletter + "`");
		}
        
        
    }
}

namespace CitySearch
{
    using System.Collections.Generic;

    public interface ICityResult
    {
        ICollection<string> NextLetters { get; set; } 

        ICollection<string> NextCities { get; set; }
    }

    public class Result : ICityResult{
        public ICollection<string> NextLetters { get; set; }
        public ICollection<string> NextCities { get; set; }

        public Result() {

            /*Initialise the set for the Next Letter and NextCities.
            Defining next letter and Next cities' data type and 
            checking to eliminate any duplicated element using HashSet.*/ 
            NextLetters = new HashSet <String>();
            NextCities = new HashSet <String>();
        }
    }

    public interface ICityFinder
    {
        ICityResult Search(string searchString);
    }

    public class Finder : ICityFinder{

        //list of cities used for this application for testing.
        string[] citiesForTest = new string[] {
            "BANDUNG",
            "BANGUI",
            "BANGKOK",
            "BANGALORE",
            "LA PAZ",
            "LA PLATA",
            "LAGOS",
            "LEEDS",
            "ZARIA",
            "ZHUGHAI",
            "ZIBO"
        };

        private HashSet<String> validCities;

        public Finder(){
            /* Defining validcity hashset by passing an array of all strings in citiesForTest to it*/
            validCities = new HashSet <String>(citiesForTest);
        }

        /* Not taking into account backspaces. Assuming that for each search, we are always increasing characters. 
        takes into account all marching cities to the character the user types, and eliminates all other 
        cities from ValidCity. This will reduce runtime in a cases where large string data is used. */
        public ICityResult Search(string searchString){
            CitySearch.ICityResult cityResult = new CitySearch.Result();
            
            // Initialize tempResult.
            HashSet<String> tempResult = new HashSet<String>();

            // Filtering valid cities into temp collection
            foreach (string nameOfCity in validCities){

                if (nameOfCity.StartsWith(searchString))
                {
                    tempResult.Add(nameOfCity);
                }
            }
            validCities = tempResult;

            // Set Result Next cities to be valid cities
            cityResult.NextCities = validCities;

            // Get next letters for each city and since it's a hash set, we know we will have uniqueness
            foreach(string validCity in validCities ){
                cityResult.NextLetters.Add(validCity[searchString.Length].ToString());
            }

            return cityResult;       
        }
    }
}

/** 
    // This method still works efficiently and takes into account "backspace", but does not take into account, the previous character that the user types.
    // Requires more runtime compared to the other highlighted method, because it goes through each string array everytime each letter is typed.

    foreach (string nameOfCity in citiesForTest){
        
        if(nameOfCity.StartsWith(searchString)){
            cityResult.NextCities.Add(nameOfCity);

            //if search string is "LA", its length = 2, however, since character array starts counting from "0", it would add the next character in that 
            //particular nameOfCity array that meets the "if condition", incase "G" and " " .
            //Also, each of these char value would need to be converted to a string for using .ToString()
            
            cityResult.NextLetters.Add(nameOfCity[searchString.Length].ToString());

        }
        
    };

    return cityResult;
          
**/