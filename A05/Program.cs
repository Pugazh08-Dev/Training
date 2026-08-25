// -----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -----------------------------------------------------------------------------------------
// Program.cs
// Program to solve the 8 Queens problem and find all possible solutions.
// Also eliminates identical solutions caused by rotations and mirror images.

using System.Text;
using static System.Console;

const int N = 8;
int[] board = new int[N];
List<int[]> solutions = new ();

Find (0);
OutputEncoding = Encoding.UTF8;
List<int[]> unique = new ();
HashSet<string> found = new ();

foreach (int[] solution in solutions) {
   int[] current = solution;
   string smallest = "";
   for (int r = 0; r < 4; r++) {
      for (int m = 0; m < 2; m++) {
         string key = string.Join (",", current);
         if (smallest == "" || key.CompareTo (smallest) < 0) smallest = key;
         int[] mirror = new int[N];
         for (int i = 0; i < N; i++)
            mirror[i] = N - 1 - current[i];
         current = mirror;
      }
      int[] rotate = new int[N];
      for (int i = 0; i < N; i++)
         rotate[current[i]] = N - 1 - i;
      current = rotate;
   }
   if (found.Add (smallest)) unique.Add (solution);
}
WriteLine ($"Canonical Solutions : {unique.Count} \n");

for (int i = 0; i < unique.Count; i++) {
   WriteLine ($"Solution {i + 1} of {unique.Count}");
   WriteLine ("┌───┬───┬───┬───┬───┬───┬───┬───┐");
   for (int row = 0; row < N; row++) {
      for (int col = 0; col < N; col++) {
         Write ("\u2502 ");
         Write (unique[i][row] == col ? "\u2655" : "\u25a1");
         Write (" ");
      }
      WriteLine ("\u2502");
      if (row < N - 1)
         WriteLine ("├───┼───┼───┼───┼───┼───┼───┼───┤");
   }
   WriteLine ("└───┴───┴───┴───┴───┴───┴───┴───┘");
   ReadKey ();
   Clear ();
}

// Find all solutions
void Find (int row) {
   if (row == N) {
      solutions.Add ((int[])board.Clone ());
      return;
   }
   for (int col = 0; col < N; col++) {
      bool safe = true;
      for (int previous = 0; previous < row; previous++) {
         if (board[previous] == col || Math.Abs (board[previous] - col) == row - previous) {
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