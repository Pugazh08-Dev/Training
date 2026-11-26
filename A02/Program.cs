// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Write a program to implement a simple _guessing game_. The computer thinks of a random
// number between 1 and 100, and the user has to guess it. The user can enter an number, and
// the computer will respond with one of these:
// Your guess is too high
// Your guess is too low
// You guessed correctly
// ------------------------------------------------------------------------------------------------
namespace A02;
using static Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   static void Main () {
      WriteLine ("Welcome to the Guessing Game \nPlease select the mode of the game. Enter 1 for" +
         " Computer Guesses, 2 for User Guesses, 3 to Exit");
      int mode;
      for (; ; ) {
         if (int.TryParse (Console.ReadLine (), out mode)) {
            switch (mode) {
               case 1:
                  WriteLine ("You chose 'Computer Guesses' mode. The computer will try" +
                     " to guess your number.");
                  ComputerGuesses ();
                  break;
               case 2:
                  WriteLine ("You chose 'User Guesses' mode. You will try to guess the " +
                     "computer's number.");
                  UserGuesses ();
                  break;
               case 3:
                  WriteLine ("You chose 'Exit' mode. bye!");
                  return;
               default:
                  WriteLine ("Invalid input. Please enter 1, 2, or 3.");
                  break;
            }
         } else WriteLine ("Invalid input. Please enter 1, 2, or 3..");
      }
   }

   #region Methods --------------------------------------------------
   static void ComputerGuesses () {
      int low = 1;
      int high = 100;
      int attempts = 0;
      WriteLine ($"Please think of a number between {low} to {high} and hit Enter");
      ReadLine ();
      while (attempts < sMaxAttempts && low <= high) {
         int guess = (low + high) / 2;
         WriteLine ($"Guess is {guess} \nEnter H for High, L for Low, C for Correct");
         WriteLine ();
         string response = ReadLine ()?.Trim ().ToUpper () ?? "";
         if (response != "H" && response != "L" && response != "C") {
            WriteLine ("Invalid input. Please enter H, L, or C.");
            continue;
         }
         attempts++;
         if (response == "H") high = guess - 1;
         else if (response == "L") low = guess + 1;
         else {
            WriteLine ($"Your Guess is Correct and achieved in {attempts} attempts");
            return;
         }
      }
      WriteLine ("Sorry, the computer could not guess number within allowed attempts.");
   }

   static void UserGuesses () {
      int low = 1;
      int high = 100;
      int attempts = 0;
      Random r = new ();
      int randomNumber = r.Next (1, 101);
      while (attempts < sMaxAttempts && low <= high) {
         WriteLine ($"Range {low} - {high} \nEnter Your Guess Now");
         if (!int.TryParse (Console.ReadLine (), out int guess)) {
            WriteLine ("Enter a Valid Value");
            continue;
         }
         if (guess < low || guess > high) {
            WriteLine ($"Please enter a number between {low} and {high}");
            continue;
         }
         attempts++;
         if (guess < randomNumber) {
            WriteLine ("Your Guess is Low");
            low = guess + 1;
         } else if (guess > randomNumber) {
            WriteLine ("Your Guess is High");
            high = guess - 1;
         } else {
            WriteLine ($"Your guess is correct and achieved in {attempts} attempts");
            return;
         }
      }
      WriteLine ($"Sorry, you could not guess the number within the allowed attempts. " +
         $"The correct number was {randomNumber}");
   }
   #endregion

   static int sMaxAttempts = 7;
}
#endregion