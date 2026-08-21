using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using static System.Console;

OutputEncoding = System.Text.Encoding.UTF8;
int size = 8;

List<int[]> solutions = new ();

int[] board = new int[size];
FindSolutions (board, 0);

WriteLine ($"Total Solutions : {solutions.Count} \n");
WriteLine ("All Solutions :");
PrintSolutions (solutions);

List<int[]> uniquesolutions = FindUniqueSolutions ();

WriteLine($"Unique Solutions : {uniquesolutions.Count} \n");
WriteLine ("12 Canonical Solutions :");
PrintSolutions(uniquesolutions);


void FindSolutions (int[] board, int row) {
   if (row == size) {
      solutions.Add ((int[])board.Clone ());
      return;
   }

   for (int i = 0; i < size; i++) {
      if(IsSafe(board, row, i)) {
         board[row] = i;
         FindSolutions (board, row + 1);
      }
   }
}


bool IsSafe (int[] board, int row, int col) {
   for(int prevrow = 0; prevrow < row; prevrow++) {
      int prevcol = board[prevrow];

      if (prevcol == col) return false;
      if (Math.Abs (prevrow - row) == Math.Abs (prevcol - col)) return false;
   }
   return true;
}

void PrintSolutions (List<int[]> boards) {
   int num = 1;
   foreach (int[] board in boards) {
      WriteLine ($"Solution {num++}:");
      PrintBoard (board);
      WriteLine ();
   }
}

void PrintBoard (int[] board) {
   for (int row = 0; row < size; row++) {
      for (int col = 0; col < size; col++) {
         WriteLine (board[row] == col ? "\u2655" : "\u25a1");
      }
      WriteLine ();
   }
}

List<int[]> FindUniqueSolutions () {
   List<int[]> uniqueSolutioms = new ();
   HashSet<string> found = new ();
   foreach (int[] solution in solutions) {
      string smallest = GetSmallestVariation (solution);
      if (found.Add (smallest)) uniqueSolutioms.Add (solution);
   }
   return uniqueSolutioms;
}

string GetSmallestVariation (int[] board) {
   string smallest = "";
   int[] current = board;
   for (int i = 0; i < 4; i++) {
      string normal = GetKey (current);
      string mirror = GetKey (Mirror (current));
      if (smallest == null || normal.CompareTo (smallest) < 0)
         smallest = normal;
      if (mirror.CompareTo (smallest) < 0) smallest = mirror;
      current = Rotate (current);
   }
   return smallest;
}

int[] Rotate (int[] board) {
   int[] rotated = new int[size];
   for (int row = 0; row < size; row++) {
      int column = board[row];
      rotated[column] = size - 1 - row;
   }
   return rotated;
}

int[] Mirror (int[] board) {
   int[] mirrored = new int[size];
   for(int row = 0; row < size; row++) 
      mirrored[row] = size - 1 - board[row];

   return mirrored;
}

string GetKey (int[] borad) => string.Join (",", borad);