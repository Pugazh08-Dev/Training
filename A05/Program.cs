using System.Text;
using static System.Console;

const int N = 8;
int[] board = new int[N];
List<int[]> solutions = new ();

Find (0);
OutputEncoding = Encoding.UTF8;
WriteLine ($"Total Solutions : {solutions.Count}");
WriteLine ();
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
   WriteLine ("\u250c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u252c\u2500\u2500\u2500\u2510");
   for (int row = 0; row < N; row++) {
      for (int col = 0; col < N; col++) {
         Write ("\u2502 ");
         Write (unique[i][row] == col ? "\u2655" : "\u25a1");
         Write (" ");
      }
      WriteLine ("\u2502");
      if (row < N - 1)
         WriteLine ("\u251c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u253c\u2500\u2500\u2500\u2524");
   }
   WriteLine ("\u2514\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2534\u2500\u2500\u2500\u2518");
   ReadKey ();
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