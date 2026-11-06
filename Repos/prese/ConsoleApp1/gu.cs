
// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------

using System;

namespace A02; // Places this class inside the A02 namespace.

class Program {
   static void Main () {
      // Display the welcome message and available game modes.
      Console.WriteLine ("Welcome to the Guessing Game");
      Console.WriteLine ("Please select the mode of the game. Enter 1 for Computer Guesses, 2 for User Guesses, 3 to Exit");

      int mode; // Stores the user's selected mode.

      // Keep showing the menu until the user chooses to exit.
      for (; ; ) {
         // Try to convert the user's input into an integer.
         if (int.TryParse (Console.ReadLine (), out mode)) {

            // Execute the selected mode.
            switch (mode) {
               case 1:
                  // The computer tries to guess the number chosen by the user.
                  Console.WriteLine ("You chose 'Computer Guesses' mode. The computer will try to guess your number.");
                  ComputerGuesses ();
                  break; // Exit the switch, then return to the menu.

               case 2:
                  // The user tries to guess the computer's random number.
                  Console.WriteLine ("You chose 'User Guesses' mode. You will try to guess the computer's number.");
                  UserGuesses ();
                  break; // Exit the switch, then return to the menu.

               case 3:
                  // End the program.
                  Console.WriteLine ("You chose 'Exit' mode. bye!");
                  return;

               default:
                  // Handle integer inputs other than 1, 2, or 3.
                  Console.WriteLine ("Invalid input. Please enter 1, 2, or 3.");
                  break;
            }
         } else {
            // Handle non-integer input, such as letters or symbols.
            Console.WriteLine ("Invalid input. Please enter 1, 2, or 3.");
         }
      }
   }

   static void ComputerGuesses () {
      int low = 1;       // Lowest possible number.
      int high = 100;    // Highest possible number.
      int attempts = 0;  // Counts valid guesses made by the computer.

      // Ask the user to think of a number without revealing it.
      Console.WriteLine ($"Please think of a number between {low} to {high} and hit Enter");
      Console.ReadLine (); // Wait for the user to press Enter.

      // Continue while attempts remain and the search range is valid.
      while (attempts < sMaxAttempts && low <= high) {

         // Choose the middle of the current range (binary search).
         int guess = (low + high) / 2;

         Console.WriteLine ($"Guess is {guess}");
         Console.Write ("Enter H for High, L for Low, C for Correct");
         Console.WriteLine ();

         // Read the response, remove surrounding spaces, and convert to uppercase.
         // If the input is null, use an empty string.
         string response = Console.ReadLine ()?.Trim ().ToUpper () ?? "";

         // Accept only H, L, or C.
         if (response != "H" && response != "L" && response != "C") {
            Console.WriteLine ("Invalid input. Please enter H, L, or C.");
            continue; // Restart the loop without counting this attempt.
         }

         attempts++; // Count the valid response.

         if (response == "H") {
            // The computer's guess is too high; search the lower half.
            high = guess - 1;
         } else if (response == "L") {
            // The computer's guess is too low; search the upper half.
            low = guess + 1;
         } else {
            // The user confirms that the computer guessed correctly.
            Console.WriteLine ($"Your Guess is Correct and achieved in {attempts} attempts");
            return; // End this game mode.
         }
      }

      // Display this message if the computer fails within the allowed attempts
      // or if the search range becomes invalid.
      Console.WriteLine ("Sorry, the computer could not guess your number within the allowed attempts. ");
   }

   static void UserGuesses () {
      int low = 1;       // Lowest number the user can currently guess.
      int high = 100;    // Highest number the user can currently guess.
      int attempts = 0;  // Counts valid guesses.

      Random r = new ();            // Create a random number generator.
      int randomNumber = r.Next (1, 101); // Generate a number from 1 through 100.
                                          // The upper limit, 101, is exclusive.

      // Continue while attempts remain and the range is valid.
      while (attempts < sMaxAttempts && low <= high) {
         // Display the current valid range.
         Console.WriteLine ($"Range {low} - {high}");
         Console.WriteLine ("Enter Your Guess Now");

         // Convert the user's input to an integer.
         // If conversion fails, ask again without counting an attempt.
         if (!int.TryParse (Console.ReadLine (), out int guess)) {
            Console.WriteLine ("Enter a Valid Value");
            continue;
         }

         // Reject guesses outside the current range.
         if (guess < low || guess > high) {
            Console.WriteLine ($"Please enter a number between {low} and {high}");
            continue; // Invalid range guesses do not count.
         }

         attempts++; // Count the valid guess.

         if (guess < randomNumber) {
            // The guess is too low; all numbers up to this guess are eliminated.
            Console.WriteLine ("Your Guess is Low");
            low = guess + 1;
         } else if (guess > randomNumber) {
            // The guess is too high; all numbers above the new limit are eliminated.
            Console.WriteLine ("Your Guess is High");
            high = guess - 1;
         } else {
            // The guess matches the generated number.
            Console.WriteLine ($"Your guess is correct and achieved in {attempts} attempts");
            return; // End this game mode.
         }
      }

      // Display the answer if the user runs out of attempts or the range becomes invalid.
      Console.WriteLine ($"Sorry, you could not guess the number within the allowed attempts. The correct number was {randomNumber}");
   }

   // Shared limit for the maximum number of valid guesses in either mode.
   static int sMaxAttempts = 7;
}
