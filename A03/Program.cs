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
using System.IO;
using System.Collections.Generic;

char[] letters = [ 'U', 'X', 'A', 'L', 'T', 'N', 'E' ];
List<(int score, string word)> results = [];
int totalScore = 0;

foreach (string line in File.ReadAllLines ("words 1.txt")) {
   string word = line.Trim ().ToUpper ();
   if (word.Length < 4 || !word.Contains ('U')) continue;
   if (!IsValidWord (word)) continue;
   int score = CalculateScore (word);
   totalScore += score; results.Add ((score, word));
}

int maxScore = results.Max (x => x.score);

foreach (var (score, word) in results.OrderByDescending (x => x.score).ThenBy (x => x.word)) {
   if(score == maxScore)
      Console.ForegroundColor = ConsoleColor.Green;

   Console.WriteLine ($"{score, 2}. {word}");
   Console.ResetColor ();
}
Console.WriteLine ($"---- \n{totalScore} Total");

int CalculateScore (string word) {
   int score = word.Length == 4 ? 1 : word.Length;
   if (IsPangram (word))
      score += 7;
   return score;
}

bool IsValidWord (string word) => word.All (c => letters.Contains (c));

bool IsPangram (string word) => letters.All (c => word.Contains (c));