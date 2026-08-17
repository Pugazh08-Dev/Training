// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to implement a word puzzle using seven given letters,
// with one mandatory letter and pangram scoring.
// ------------------------------------------------------------------------------------------------

using System;
using System.Linq;

char[] letters = new char[] { 'U', 'X', 'A', 'L', 'T', 'N', 'E' };

int totalscore = 0;

foreach (string line in System.IO.File.ReadAllLines ("C:\\Users\\murugesanpu\\Downloads\\words 1.txt")) {
   string word = line.Trim ().ToUpper ();
   if (word.Length < 4 || !word.Contains ('U')) continue;
   if (!IsValidWord (word)) continue;
   int score = CalculateScore (word);
   totalscore += score; Console.WriteLine ($"{word} - Score: {score}");
}
Console.WriteLine ($"Total Score: {totalscore}");

int CalculateScore (string word) {
   int score = word.Length == 4 ? 1 : word.Length;
   if (IsPangram (word)) {
      score += 7;
   }
   return score;
}

bool IsValidWord (string word) {
   return word.All (c => letters.Contains (c));
}

bool IsPangram (string word) {
   return letters.All (c => word.Contains (c));
}