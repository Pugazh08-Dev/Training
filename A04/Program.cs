//-----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------

// Program.cs
// Write a program to find the occurences of a given character in a Words.txt file.
// The program should read the file, count the number of times the specified character appears, and display the result to the user.
// ------------------------------------------------------------------------------------------------

using static System.Console;

var charCount = new Dictionary<char, int> ();
foreach (var line in File.ReadLines ("words 1.txt")) {
   foreach (var c in line.ToUpper ()) {
      if (char.IsLetter (c)) {
         if (charCount.TryGetValue (c, out int value)) charCount[c] = ++value;
         else charCount[c] = 1;
      }
   }
}
WriteLine ("Top 7 character counts:");

foreach (var kvp in charCount.OrderByDescending (kvp => kvp.Value).Take (7))
   WriteLine ($"Letter '{kvp.Key}': {kvp.Value} occurrences");