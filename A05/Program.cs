// -----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -----------------------------------------------------------------------------------------
// Program.cs
// Program to solve the 8 Queens problem and find all possible solutions.
// Also eliminates identical solutions caused by rotations and mirror images.

using System.Text;
using static System.Console;

OutputEncoding = Encoding.UTF8;

const int N = 8;
int[] board = new int[N];
List<int[]> allsolutions = [];
Find (0);
List<int[]> uniquesolutions = [];
HashSet<long> uniqueSolutionKeys = [];

// Check every solution to remove rotations and mirror images.
foreach (int[] solution in allsolutions) {
   int[] current = solution;
   long smallest = long.MaxValue;
   // Generate all four rotations of the solution.
   for (int r = 0; r < 4; r++) {
      // Check both the original and mirror image.
      for (int m = 0; m < 2; m++) {
         long number = 0;
         // Convert the current board arrangement into a number for comparison.
         foreach (int col in current) number = number * 10 + col;
         if (number < smallest) smallest = number;
         int[] mirror = new int[N];
         // Reverse the column positions to create the mirror.
         for (int i = 0; i < N; i++) mirror[i] = N - 1 - current[i];
         current = mirror;
      }
      int[] rotate = new int[N];
      // Move every queen to its new position after rotation.
      for (int i = 0; i < N; i++)
         rotate[current[i]] = N - 1 - i;
      current = rotate;
   }
   if (uniqueSolutionKeys.Add (smallest)) uniquesolutions.Add (solution);
}
List<int[]> solutionsToPrint;
string? choice;
while (true) {
   WriteLine ("8 Queens Problem \n------- \n1. Print all solutions \n2. Print canonical solutions");
   Write ("Enter your choice: ");
   choice = ReadLine ();
   if (choice == "1") {
      solutionsToPrint = allsolutions;
      break;
   } else if (choice == "2") {
      solutionsToPrint = uniquesolutions;
      break;
   } else WriteLine ("Invalid choice. Please enter 1 or 2.\n");
}
Clear ();
WriteLine ($"Solutions : {solutionsToPrint.Count}");
WriteLine ();
for (int i = 0; i < solutionsToPrint.Count; i++) {
   WriteLine ($"Solution {i + 1} of {solutionsToPrint.Count}");
   WriteLine ("┌───┬───┬───┬───┬───┬───┬───┬───┐");
   for (int row = 0; row < N; row++) {
      for (int col = 0; col < N; col++) {
         Write ("\u2502 ");
         Write (solutionsToPrint[i][row] == col ? "\u2655" : "\u25a1");
         Write (" ");
      }
      WriteLine ("\u2502");
      if (row < N - 1) WriteLine ("├───┼───┼───┼───┼───┼───┼───┼───┤");
   }
   WriteLine ("└───┴───┴───┴───┴───┴───┴───┴───┘");
}

#region Method ------------------------------------------------------
// Find all solutions
void Find (int row) {
   if (row == N) {
      allsolutions.Add ((int[])board.Clone ());
      return;
   }
   bool safe;
   // Try every column in the current row.
   for (int col = 0; col < N; col++) {
      safe = true;
      // Check the current position against previous queens.
      for (int previous = 0; previous < row; previous++) {
         int previousColumn = board[previous];
         if (previousColumn == col || Math.Abs (previousColumn - col) == row - previous) {
            safe = false;
            break;
         }
      }
      if (safe) {
         board[row] = col;
         Find (row + 1);
      }
   }
}
#endregion