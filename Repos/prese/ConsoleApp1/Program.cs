
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

// Define the permitted letters for the puzzle.
char[] letters = ['U', 'X', 'A', 'L', 'T', 'N', 'E'];

// Store each valid word with its score.
List<(int Score, string Word)> results = [];
int totalScore = 0; // Accumulate the total score.

// Read the dictionary file and process each word.
foreach (string line in File.ReadAllLines ("words 1.txt")) {
   string word = line.Trim ().ToUpper (); // Normalize input.

   // Reject short words and words missing the mandatory letter U.
   if (word.Length < 4 || !word.Contains ('U')) continue;

   // Reject words containing characters outside the permitted set.
   if (!IsValidWord (word)) continue;

   int score = CalculateScore (word); // Calculate the word score.
   totalScore += score;              // Update the cumulative score.
   results.Add ((score, word));       // Store the score-word pair.
}

// Find the highest individual word score.
int maxScore = results.Max (x => x.Score);

// Sort by score descending, then alphabetically.
foreach (var (Score, Word) in results
   .OrderByDescending (x => x.Score).ThenBy (x => x.Word)) {

   // Highlight the highest-scoring words in green.
   if (Score == maxScore) Console.ForegroundColor = ConsoleColor.Green;
   Console.WriteLine ($"{Score,2}. {Word}"); // Display the result.
   Console.ResetColor (); // Restore the default console color.
}

// Display the total accumulated score.
Console.WriteLine ($"---- \n{totalScore} Total");

// Calculate the score, including any pangram bonus.
int CalculateScore (string word) {
   int score = word.Length == 4 ? 1 : word.Length;
   if (IsPangram (word)) score += 7;
   return score;
}

// Check whether every character belongs to the permitted set.
bool IsValidWord (string word) => word.All (c => letters.Contains (c));

//This is an expression - bodied local function that returns a Boolean value.
//- All(): Checks whether every character satisfies a condition.
//- c => ...: A lambda expression representing the current character.
//- letters.Contains(c): Checks whether that character belongs to the allowed array.
//If even one character is not permitted, All() returns false.
//For example, TAXI is invalid because I is not in the permitted set.

// Check whether the word contains all seven permitted letters.
bool IsPangram (string word) => letters.All (c => word.Contains (c));

//This checks whether every permitted character occurs at least once in the word.

//The important difference is:
//-IsValidWord () checks whether every character in the word is allowed.
//- IsPangram() checks whether every allowed character appears in the word.


//“My program solves a word puzzle using seven permitted letters, with U as the mandatory letter.
//First, it reads words from a text file and validates them. Then it calculates each word's score, adds a bonus for pangrams, and stores the results in a list of named tuples.
//Finally, it sorts the words by score in descending order, highlights the highest-scoring words, and displays the total score.
//I use LINQ for validation and sorting, and separate local functions for scoring and word validation to improve code readability and maintainability.”


//The time complexity is \(O(nm + k\log k)\), where \(n\) is the number of input words, \(m\) is
//the average word length, and \(k\) is the number of valid words, because validation takes linear time and sorting takes \(O(k\log k)\).












using static System.Console; // Import console methods without the Console prefix.

// Store each character and its occurrence count.
var charCount = new Dictionary<char, int> ();

// Read the file line by line.
foreach (var line in File.ReadLines ("words 1.txt")) {

   // Convert each line to uppercase and iterate through its characters.
   foreach (var c in line.ToUpper ()) {

      // Process letters only, ignoring spaces, digits, and punctuation.
      if (char.IsLetter (c)) {

         // Check whether the character already exists in the dictionary.
         if (charCount.TryGetValue (c, out int value))
            charCount[c] = ++value; // Increment the existing count.
         else
            charCount[c] = 1; // Initialize the count for a new character.
      }
   }
}

// Display the heading for the output.
WriteLine ("Top 7 character counts:");

// Sort by frequency in descending order and select the top seven entries.
foreach (var kvp in charCount.OrderByDescending (kvp => kvp.Value).Take (7))

   // Display the character and its total occurrences.
   WriteLine ($"Letter '{kvp.Key}': {kvp.Value} occurrences");
