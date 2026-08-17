//-----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026. 
// Copyright (c) Metamation India. 
// ------------------------------------------------------------------------------------------------- 

// Program.cs 
// Write a program to find the occurences of a given character in a Words.txt file.
// The program should read the file, count the number of times the specified character appears, and display the result to the user.	 
// ------------------------------------------------------------------------------------------------ 

var charCount = new Dictionary<char, int> {
   ['U'] = 0,
   ['X'] = 0,
   ['A'] = 0,
   ['L'] = 0,
   ['T'] = 0,
   ['N'] = 0,
   ['E'] = 0
};

foreach (var line in File.ReadLines ("C:\\Users\\murugesanpu\\Downloads\\words 1.txt")) {
   foreach (var c in line) {
      if (charCount.ContainsKey (c)) {
         charCount[c]++;
      }
   }
}

Console.WriteLine ("Here, is the count of the characters in the file:");
foreach (var kvp in charCount) {
   Console.WriteLine ($"Character '{kvp.Key}': {kvp.Value} occurrences");
}
