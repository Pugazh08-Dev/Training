//-----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------

// Program.cs
// Write a program to find the occurences of a given character in a Words.txt file.
// The program should read the file, count the number of times the specified character appears, and display the result to the user.
// ------------------------------------------------------------------------------------------------

using static System.Console;

var charCount = "UXALTNE".ToDictionary (c => c, c => 0);

foreach (var line in File.ReadLines ("words 1.txt")) {
   foreach (var c in line) {
      if (charCount.TryGetValue (c, out int count)) charCount[c] = count + 1;
   }
}

WriteLine ("Character counts in the file:");

foreach (var kvp in charCount.OrderByDescending (kvp => kvp.Value))
   WriteLine ($"Character '{kvp.Key}': {kvp.Value} occurrences");