
// How to reverse a string
// String str = "hello world";
// char[] ArrStr = str.ToCharArray();
// char[] RevArr = new char[str.Length];
// int j = 0;
// for(int i = ArrStr.Length-1;i >=0; i--)
// {
//     RevArr[j] = ArrStr[i];
//     j++;
// }
// Console.WriteLine(RevArr);


// How to find if the given string is a palindrome or not?

// string str1 = "wllw";
// string str2 = "wlw";
// char[] ArrStr1 = str1.ToCharArray();
// char[] ArrStr2 = str2.ToCharArray();

// int j = ArrStr2.Length -1;
// bool palindrome = true;
// for (int i = 0; i < ArrStr2.Length; i++)
// {
//     if (ArrStr1[i] != ArrStr2[j])
//     {
//         palindrome = false;
//         break;
//     }
//     j--;
// }
// if (palindrome)
// {
//     Console.WriteLine("palindrom");
// }
// else
// {
//         Console.WriteLine("not palindrom");

// }



// Q.3: How to reverse the order of words in a given string?
// Ans.: The user will input a sentence and we need to reverse the sequence of words in the sentence.

// input: Welcome to Csharp corner, output: corner Csharp to Welcome



// string str = "Welcome to Csharp corner";
// var strpiece= str.Split(" ");
// string reserver ="";
// foreach (var item in strpiece)
// {
//     reserver = item + " " + reserver;
// }
// Console.WriteLine(reserver);


// Q.4: How to reverse each word in a given string?
// Ans.: The user will input a sentence and we need to reverse each word individually without changing its position in the sentence.

// input: Welcome to Csharp corner, output: emocleW ot prahsC renroc



// string str = "Welcome to Csharp corner";

// var strpiece= str.Split(" ");

// var newString = "";

// foreach (var item in strpiece)
// {
//     char[] Wordarray = new char[item.Length];
//     string reversedWord = "";
//     for (int i = 0; i < item.Length; i++)
//     {
//         Wordarray[i] = item[item.Length - 1 - i];
//         reversedWord = reversedWord + Wordarray[i];
//     }
//     newString = newString +  reversedWord + " " ;
// }
// Console.WriteLine(newString);


// Q.5: How to count the occurrence of each character in a string?

// Dictionary<char, int> CharCount = new Dictionary<char, int>();

// string str = "hello world";
// char[] strArray = str.ToCharArray();
// foreach(char item in strArray)
// {
//     if (item != ' ')
//     {
//       if (CharCount.ContainsKey(item))
//     {
//         CharCount[item] = CharCount[item] + 1;
//     }
//     else
//     {
//         CharCount.Add(item, 1);
//     }   
//     }
// }
// foreach(KeyValuePair<char,int> item in CharCount)
// {
//     Console.WriteLine($"{item.Key} - {item.Value}");
// }

// Q.6: How to remove duplicate characters from a string?
// string str = "csharpcorner";
// string result = string.Empty;
// for(int i =0; i < str.Length; i++)
// {
//     if (!result.Contains(str[i]))
//     {
//         result = result + str[i];
//     }
// }
// Console.WriteLine(result);

























// using cSharpConcepts;
// class Program
// {
//     public static void Main(string[] args)
//     {
//         DicPractice dict = new DicPractice();
//         dict.AddData();
//         dict.PrintData();
//         // Console.WriteLine(dict.dict.Count());
//         // dict.dict.Remove("abdullah");
//         foreach(string Key in dict.dict.Keys)
//         {
//             Console.WriteLine(Key);
//         }
//         // Console.WriteLine(dict.dict.Keys);


//     }
// }


