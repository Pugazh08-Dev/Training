// ------------------------------------------------------------------------------------------------ 
// Training ~ A training program for new joinees at Metamation, Batch- July 2026. 
// Copyright (c) Metamation India. 
// ------------------------------------------------------------------------------------------------- 

// Program.cs 
// Write a program to implement a simple _guessing game_. The computer thinks of a random
// number between 1 and 100, and the user has to guess it. The user can enter an number, and
// the computer will respond with one of these:
// Your guess is too high
// Your guess is too low
// You guessed correctly 	 
// ------------------------------------------------------------------------------------------------ 

namespace A02;

class Program {
   static int low = 1;
   static int high = 100;
   static int attempts = 0;
   static int maxattempts = 7;

   static void Main (string[] args) {
      Console.WriteLine ("Welcome to the Guessing Game");
      Console.WriteLine ("Please select the mode of the game. Enter 1 for Computer Guesses, 2 for User Guesses");

      int mode;
      for (; ; ) {
         if (int.TryParse (Console.ReadLine (), out mode)) {
            switch (mode) {
               case 1:
                  Console.WriteLine ("You chosen Computer Guesses mode. The computer will try to guess your number.");
                  ComputerGuesses ();
                  break;
               case 2:
                  Console.WriteLine ("You chosen User Guesses mode. You will try to guess the computer's number.");
                  UserGuesses ();
                  break;
               default:
                  Console.WriteLine ("Invalid input. Please enter 1 or 2.");
                  continue;
            }
         } else {
            Console.WriteLine ("Invalid input. Please enter a number.");
         }
      }
   }

   static void ComputerGuesses () {
      Console.WriteLine ($"Please Think the number between {low} to {high} and hit Enter");
      Console.ReadLine ();

      while (attempts < maxattempts) {
         if (low <= high) {
            int guess = (low + high) / 2;
            attempts++;

            Console.WriteLine ($"Guess is {guess}");
            Console.Write ("Enter H for High, L for Low, C for Correct");
            Console.WriteLine ();

            string? response = Console.ReadLine ();
            if (response != null) {
               response = response.Trim ().ToUpper ();
            } else {
               Console.WriteLine ("Invalid input. Please enter H, L, or C.");
               continue;
            }

            if (response == "H") {
               high = guess - 1;
            } else if (response == "L") {
               low = guess + 1;
            } else if (response == "C") {
               Console.WriteLine ($"Your Guess is Correct and acheived in {attempts} attempts");
               pause ();
               break;
            }
         }
      }
   }

   static void UserGuesses () {
      Random r = new Random ();
      int randomNumber = r.Next (1, 100);

      while (attempts <= maxattempts) {
         Console.WriteLine ($"Range {low} - {high}");
         Console.WriteLine ("Enter Your Guess Now");

         if (!int.TryParse (Console.ReadLine (), out int guess)) {
            Console.WriteLine ("Enter a Valid Value");
         }

         attempts++;

         if (guess < randomNumber) {
            Console.WriteLine ("Your Guess is Low");

            if (guess >= low) {
               low = guess + 1;
            }
         } else if (guess > randomNumber) {
            Console.WriteLine ("Your Guess is High");

            if (guess <= high) {
               high = guess - 1;
            }
         } else {
            Console.WriteLine ($"Your Guess is Correct and acheived in {attempts} attempts");
            pause ();
            break;
         }
      }
   }

   static void pause () {
      Console.WriteLine ("Press any key to continue...");
      Console.ReadLine ();
   }
}
