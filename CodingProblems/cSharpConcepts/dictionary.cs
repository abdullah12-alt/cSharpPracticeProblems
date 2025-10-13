using System.Collections.Generic;
namespace cSharpConcepts
{
    public class DicPractice
    {
        public Dictionary<string, int> dict = new Dictionary<string, int>();

        public void AddData()
        {
            dict.Add("abdullah", 1);
            dict.Add("ali", 2);
            dict.Add("abid", 3);
        }
        public void PrintData()
        {
            foreach (KeyValuePair<string, int> items in dict)
            {
                Console.WriteLine($"This is {items.Key} and id is {items.Value}");

            }
        }


    }
}


// Create a dictionary with string key and Int16 value pair
// Dictionary < string, Int16 > AuthorList = new Dictionary < string, Int16 > ();
// AuthorList.Add("Mahesh Chand", 35);
// AuthorList.Add("Mike Gold", 25);
// AuthorList.Add("Praveen Kumar", 29);
// AuthorList.Add("Raj Beniwal", 21);
// AuthorList.Add("Dinesh Beniwal", 84);
// // Count
// Console.WriteLine("Count: {0}", AuthorList.Count);
// // Set Item value
// AuthorList["Neel Beniwal"] = 9;
// if (!AuthorList.ContainsKey("Mahesh Chand")) {
// AuthorList["Mahesh Chand"] = 20;
// }
// if (!AuthorList.ContainsValue(9)) {
// Console.WriteLine("Item found");
// }
// // Read all items
// Console.WriteLine("Authors all items:");
// foreach(KeyValuePair < string, Int16 > author in AuthorList) {
// Console.WriteLine("Key: {0}, Value: {1}", author.Key, author.Value);
// }
// // Remove item with key = 'Mahesh Chand'
// AuthorList.Remove("Mahesh Chand");
// // Remove all items
// AuthorList.Clear();