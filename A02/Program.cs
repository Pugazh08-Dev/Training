// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------
// Program.cs
// Program to implement a simple guessing game with three modes:
// 1. Computer guesses using Binary Search.
// 2. User guesses the computer's random number.
// 3. Computer guesses using Reverse Method.
// -------------------------------------------------------------------------------------------------

using System;

while(true) {
   Console.WriteLine("Welcome to the Guessing Game!");
   Console.WriteLine("Choose a mode: 1 - Computer guesses, 2 - User guesses, 3 - Computer guesses (Reverse Method)");
   Console.WriteLine("Enter your choice (1, 2, or 3):");
   Console.WriteLine("Enter 'e' to quit the game.");
   Console.WriteLine("---------------------------------------------------");

   string input = Console.ReadLine () ?? "";
   if(input.ToLower () == "e") {
      break;
   }

   if (!int.TryParse(input, out int mode)) {
      Console.WriteLine("Invalid input. Please enter a valid mode (1, 2, or 3).");
      continue;
   }

   switch (mode) {
      case 1:
         ComputerGuesses ();
         break;
      case 2:
         UserGuesses ();
         break; 
      case 3:
         ReverseGuesses ();
         break;
      default:
         Console.WriteLine("Invalid mode selected. Please choose 1, 2, or 3.");
         break;
   }
}

void ComputerGuesses () {
   int low = 1;
   int high = 100;
   int attempts = 0;
   int maxattempts = 7;


   Console.Write ("Think of a number between 1 and 100, and hit Enter");
   Console.ReadLine ();

   while(attempts < maxattempts) {
      int guess = (low + high) / 2;
      attempts++;
      Console.WriteLine ($"Computer guesses: {guess}. Is it correct? (y - yes/n - no)");
      string response = Console.ReadLine () ?? "";
      if (response.ToLower () == "y") {
         Console.WriteLine ($"Computer guessed your number {guess} in {attempts} attempts!");
         return;
      } else {
         Console.WriteLine ("Is your number higher or lower than the guess? (Enter h for higher/l for lower)");
         response = Console.ReadLine () ?? "";
         if (response.ToLower () == "h") {
            low = guess + 1;
         } else if (response.ToLower () == "l") {
            high = guess - 1;
         } else {
            Console.WriteLine ("Invalid response. Please answer with 'h' or 'l'.");
            attempts--;
         }
      }
   }
}

void UserGuesses () {
   Random rand = new Random ();
   int numberToGuess = rand.Next (1, 101);
   int low = 1;
   int high = 100;
   int attempts = 0;
   int maxattempts = 7;

   while (attempts < maxattempts) {
      Console.WriteLine ($"I have selected a number between {low} and {high}. Try to guess it!");

      if (!int.TryParse (Console.ReadLine (), out int userGuess)) {
         Console.WriteLine ("Invalid input. Please enter a valid number.");
         continue;
      }
   
      attempts++;
      if (userGuess < low || userGuess > high) {
         Console.WriteLine ($"Please enter a number between {low} and {high}.");
         continue;
      }

      if (userGuess == numberToGuess) {
         Console.WriteLine ($"Congratulations! You guessed the number {numberToGuess} in {attempts} attempts!");
         pause ();
         return;
      } else if (userGuess < numberToGuess) {
         Console.WriteLine ("your guess is low. Try again.");
         low = userGuess + 1;
      } else {
         Console.WriteLine ("your guess is high. Try again.");
         high = userGuess - 1;
      }
   }
   Console.WriteLine ("you have exhausted your attempts. The number was: " + numberToGuess);
   pause ();
}

void ReverseGuesses () {
   int attempts = 0;
   int low = 1;
   int high = 100;
   int maxattempts = 7;
   Console.WriteLine ();
   Console.WriteLine ("Think of a number between 1 and 100, and hit Enter");
   Console.WriteLine ("I will try to guess from downwards.");
   Console.ReadLine ();

   while (attempts < maxattempts) {
      int guess;
      if (attempts == 0) {
         guess = high;
      } else {
         guess = (low + high) / 2; // Decrease by 15 each time
      }
      attempts++;

      Console.WriteLine ();
      Console.WriteLine ("My guess is : " + guess + ". Enter h for higher, l for lower, or c if correct.");
      string response = Console.ReadLine () ?? "";
      if (response == "c") {
         Console.WriteLine ($"I guessed your number {guess} in {attempts} attempts!");
         pause ();
         return;

      } else if (response == "h") {
         low = guess + 1;
      } else if (response == "l") {
         high = guess - 1;
      } else {
         Console.WriteLine ("Invalid response. Please enter 'h', 'l', or 'c'.");
         attempts--;
      }
   }
   Console.WriteLine ("Couldn't guess the number");
   pause ();
}

void pause() {
   Console.WriteLine ("Press any key to continue...");
   Console.ReadKey ();
}
