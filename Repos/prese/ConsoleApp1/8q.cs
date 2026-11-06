
// Import UTF-8 encoding support and Console methods.
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

// Enable Unicode characters for displaying chess queens and board borders.
OutputEncoding = Encoding.UTF8;

// Define the chessboard size.
const int N = 8;

// Store the column position of each queen, indexed by row.
int[] board = new int[N];

// Store all valid board configurations.
List<int[]> solutions = [];

// Store canonical keys to avoid duplicate solutions caused by symmetry.
HashSet<long> uniqueSolutionKeys = [];

ConsoleKey choice;
bool uniqueOnly;

// Display the problem description and available options.
WriteLine ("8 Queens Problem \n-------");
WriteLine ("Place 8 queens on an 8x8 chessboard so that no two queens attack each other.\n");
WriteLine ("1. Print all solutions");
WriteLine ("2. Print unique solutions\n");

// Read and validate the user's menu selection.
while (true) {
   Write ("Enter your choice: ");
   choice = ReadKey ().Key;
   WriteLine ();

   // Select all solutions.
   if (choice is ConsoleKey.D1 or ConsoleKey.NumPad1) {
      uniqueOnly = false;
      break;
   }

   // Select only symmetry-unique solutions.
   if (choice is ConsoleKey.D2 or ConsoleKey.NumPad2) {
      uniqueOnly = true;
      break;
   }

   // Reject invalid menu selections.
   WriteLine ("Invalid choice. Please enter 1 or 2.\n");
}

// Search for solutions and display the results.
FindSolutions (0, uniqueOnly);
PrintSolutions (solutions);

#region Methods ------------------------------------------------------

// Solve the puzzle using recursive backtracking.
void FindSolutions (int row, bool uniqueOnly) {

   // Base case: all eight rows have been assigned a queen.
   if (row == N) {
      int[] solution = (int[])board.Clone ();

      // Store every valid solution when uniqueness filtering is disabled.
      if (!uniqueOnly) {
         solutions.Add (solution);
         return;
      }

      // Generate symmetric configurations to identify equivalent solutions.
      int[] current = solution;
      long smallest = long.MaxValue;

      // Explore four rotations and two mirror states.
      for (int r = 0; r < 4; r++) {
         for (int m = 0; m < 2; m++) {

            // Encode the board configuration as a numeric key.
            long number = 0;
            foreach (int col in current)
               number = (number * 10) + col;

            // Retain the smallest key as the canonical representation.
            if (number < smallest) smallest = number;

            // Generate the mirrored configuration.
            current = GetMirrored (current);
         }

         // Rotate the configuration by 90 degrees.
         current = GetRotated (current);
      }

      // Store the solution only if its canonical key is new.
      if (uniqueSolutionKeys.Add (smallest))
         solutions.Add (solution);

      return;
   }

   // Try placing a queen in every column of the current row.
   for (int col = 0; col < N; col++) {
      bool isSafe = true;

      // Check the candidate position against previously placed queens.
      for (int previous = 0; previous < row; previous++) {
         int previousColumn = board[previous];

         // Reject positions sharing a column or diagonal.
         if (previousColumn == col ||
             Math.Abs (previousColumn - col) == row - previous) {
            isSafe = false;
            break;
         }
      }

      // Place the queen and recursively process the next row.
      if (isSafe) {
         board[row] = col;
         FindSolutions (row + 1, uniqueOnly);
      }
   }
}

// Generate the horizontal mirror image of a solution.
int[] GetMirrored (int[] solution) {
   int[] mirror = new int[N];

   // Reverse each queen's column position.
   for (int i = 0; i < N; i++)
      mirror[i] = N - 1 - solution[i];

   return mirror;
}

// Rotate a solution by 90 degrees.
int[] GetRotated (int[] solution) {
   int[] rotated = new int[N];

   // Map each queen's original coordinates to rotated coordinates.
   for (int i = 0; i < N; i++)
      rotated[solution[i]] = N - 1 - i;

   return rotated;
}

// Display the solutions as chessboards in the console.
void PrintSolutions (List<int[]> solutions) {
   WriteLine ($"\nSolutions : {solutions.Count}\n");

   // Iterate through every stored solution.
   for (int i = 0; i < solutions.Count; i++) {
      WriteLine ($"Solution {i + 1} of {solutions.Count}");
      WriteLine ("┌───┬───┬───┬───┬───┬───┬───┬───┐");

      // Render each row and column of the chessboard.
      for (int row = 0; row < N; row++) {
         for (int col = 0; col < N; col++) {
            Write ("│ ");

            // Display a queen when its column matches the stored position.
            if (solutions[i][row] == col) Write ("\u2655");
            else Write (" ");

            Write (" ");
         }

         WriteLine ("│");

         // Draw a separator between consecutive rows.
         if (row < N - 1)
            WriteLine ("├───┼───┼───┼───┼───┼───┼───┼───┤");
      }

      // Draw the bottom border of the chessboard.
      WriteLine ("└───┴───┴───┴───┴───┴───┴───┴───┘");
   }
}
#endregion
