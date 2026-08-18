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
   static void Main () {
      Console.WriteLine ("Welcome to the Guessing Game");
      Console.WriteLine ("Please select the mode of the game. Enter 1 for Computer Guesses, 2 for User Guesses, 3 to Exit");

      int mode;
      for (; ; ) {
         if (int.TryParse (Console.ReadLine (), out mode)) {
            switch (mode) {
               case 1:
                  Console.WriteLine ("You chose 'Computer Guesses' mode. The computer will try to guess your number.");
                  ComputerGuesses ();
                  break;
               case 2:
                  Console.WriteLine ("You chose 'User Guesses' mode. You will try to guess the computer's number.");
                  UserGuesses ();
                  break;
               case 3:
                  Console.WriteLine ("You chose 'Exit' mode. bye!");
                  return;
               default:
                  Console.WriteLine ("Invalid input. Please enter 1, 2, or 3.");
                  break;
            }
         } else Console.WriteLine ("Invalid input. Please enter 1, 2, or 3..");
      }
   }

   static void ComputerGuesses () {
      int low = 1;
      int high = 100;
      int attempts = 0;

      Console.WriteLine ($"Please think of a number between {low} to {high} and hit Enter");
      Console.ReadLine ();

      while (attempts < sMaxAttempts && low <= high) {
         int guess = (low + high) / 2;

         Console.WriteLine ($"Guess is {guess}");
         Console.Write ("Enter H for High, L for Low, C for Correct");
         Console.WriteLine ();

         string response = Console.ReadLine ()?.Trim ().ToUpper () ?? "";

         if (response != "H" && response != "L" && response != "C") {
            Console.WriteLine ("Invalid input. Please enter H, L, or C.");
            continue;
         }

         attempts++;

         if (response == "H") {
            high = guess - 1;
         } else if (response == "L") {
            low = guess + 1;
         } else {
            Console.WriteLine ($"Your Guess is Correct and acheived in {attempts} attempts");
            return;
         }
      }
      Console.WriteLine ("Sorry, the computer could not guess your number within the allowed attempts. ");
   }

   static void UserGuesses () {
      int low = 1;
      int high = 100;
      int attempts = 0;

      Random r = new ();
      int randomNumber = r.Next (1, 101);

      while (attempts < sMaxAttempts && low <= high) {
         Console.WriteLine ($"Range {low} - {high}");
         Console.WriteLine ("Enter Your Guess Now");

         if (!int.TryParse (Console.ReadLine (), out int guess)) {
            Console.WriteLine ("Enter a Valid Value");
            continue;
         }

         if(guess < low || guess > high) {
            Console.WriteLine ($"Please enter a number between {low} and {high}");
            continue;
         }

         attempts++;

         if (guess < randomNumber) {
            Console.WriteLine ("Your Guess is Low");
            low = guess + 1;
         } else if (guess > randomNumber) {
            Console.WriteLine ("Your Guess is High");
            high = guess - 1;
         } else {
            Console.WriteLine ($"Your guess is correct and acheived in {attempts} attempts");
            return;
         }
      }
      Console.WriteLine ($"Sorry, you could not guess the number within the allowed attempts. The correct number was {randomNumber}");
   }

   static int sMaxAttempts = 7;
}
