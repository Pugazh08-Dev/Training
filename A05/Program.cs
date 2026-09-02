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
List<int[]> allSolutions = [];
List<int[]> uniqueSolutions = [];
HashSet<long> uniqueSolutionKeys = [];

FindSolutions (0);

// Find canonical solutions
foreach (int[] solution in allSolutions) {
   int[] current = solution;
   long smallest = long.MaxValue;
   for (int r = 0; r < 4; r++) {
      for (int m = 0; m < 2; m++) {
         long number = 0;
         foreach (int col in current) number = (number * 10) + col;
         if (number < smallest) smallest = number;
         current = GetMirrored (current);
      }
      current = GetRotated (current);
   }
   if (uniqueSolutionKeys.Add (smallest)) uniqueSolutions.Add (solution);
}

List<int[]> solutionsToPrint;
ConsoleKey choice;
while (true) {
   WriteLine ("8 Queens Problem \n-------");
   WriteLine ("1. Print all solutions");
   WriteLine ("2. Print canonical solutions");
   Write ("Enter your choice: ");
   choice = ReadKey ().Key;
   WriteLine ();
   if (choice is ConsoleKey.D1 or ConsoleKey.NumPad1) {
      solutionsToPrint = allSolutions;
      break;
   }
   if (choice is ConsoleKey.D2 or ConsoleKey.NumPad2) {
      solutionsToPrint = uniqueSolutions;
      break;
   }
   WriteLine ("Invalid choice. Please enter 1 or 2.\n");
}
PrintSolutions (solutionsToPrint);

#region Method ------------------------------------------------------
// Find all solutions
void FindSolutions (int row) {
   if (row == N) {
      allSolutions.Add ((int[])board.Clone ());
      return;
   }
   for (int col = 0; col < N; col++) {
      bool isSafe = true;
      for (int previous = 0; previous < row; previous++) {
         int previousColumn = board[previous];
         if (previousColumn == col || Math.Abs (previousColumn - col) == row - previous) {
            isSafe = false;
            break;
         }
      }
      if (isSafe) {
         board[row] = col;
         FindSolutions (row + 1);
      }
   }
}

int[] GetMirrored (int[] solution) {
   int[] mirror = new int[N];
   for (int i = 0; i < N; i++) mirror[i] = N - 1 - solution[i];
   return mirror;
}

int[] GetRotated (int[] solution) {
   int[] rotated = new int[N];
   for (int i = 0; i < N; i++) rotated[solution[i]] = N - 1 - i;
   return rotated;
}

void PrintSolutions (List<int[]> solutions) {
   for (int i = 0; i < solutions.Count; i++) {
      WriteLine ($"Solution {i + 1} of {solutions.Count}");
      WriteLine ("┌───┬───┬───┬───┬───┬───┬───┬───┐");
      for (int row = 0; row < N; row++) {
         for (int col = 0; col < N; col++) {
            Write ("│ ");
            if (solutions[i][row] == col) Write ("\u2655");
            else Write (" ");
            Write (" ");
         }
         WriteLine ("│");
         if (row < N - 1) WriteLine ("├───┼───┼───┼───┼───┼───┼───┼───┤");
      }
      WriteLine ("└───┴───┴───┴───┴───┴───┴───┴───┘");
   }
}
#endregion