
// -----------------------------------------------------------------------------------------
// A07 - Implement double.Parse method
// Converts a string into a double without using the built-in double.Parse().
// -----------------------------------------------------------------------------------------

using System;
using static System.Console;

// Read numeric input from the user.
Write ("Enter a Number: ");
string? input = ReadLine ();

try {
   double result = ParseDouble (input); // Convert the input into a double.
   WriteLine ($"Result = {result}");
} catch (Exception ex) {
   WriteLine ($"Error: {ex.Message}"); // Handle invalid input.
}

#region Methods ------------------------------------------------------

// Convert a string into a double using manual parsing.
static double ParseDouble (string? input) {

   // Validate the input and remove surrounding whitespace.
   if (string.IsNullOrEmpty (input))
      throw new FormatException ("Input string is null or empty");
   input = input.Trim ();

   int index = 0;             // Track the current character position.
   bool isNegative = false;   // Store the sign of the number.

   // Read the optional positive or negative sign.
   if (index < input.Length && input[index] is '+' or '-') {
      isNegative = input[index] == '-';
      index++;
   }

   double result = 0;
   bool hasIntegerDigit = false;

   // Parse the integer part using positional multiplication.
   while (index < input.Length && char.IsDigit (input[index])) {
      result = result * 10 + (input[index] - '0');
      index++;
      hasIntegerDigit = true;
   }

   // Parse the fractional part after the decimal point.
   if (index < input.Length && input[index] == '.') {
      if (!hasIntegerDigit)
         throw new FormatException ("Digits required before decimal point");

      index++;
      if (index >= input.Length || !char.IsDigit (input[index]))
         throw new FormatException ("Digits are required after decimal point.");

      double decimalPlace = 0.1;

      // Add each fractional digit using decreasing place values.
      while (index < input.Length && char.IsDigit (input[index])) {
         result += (input[index] - '0') * decimalPlace;
         decimalPlace *= 0.1;
         index++;
      }
   }

   // Ensure that at least one integer digit was provided.
   if (!hasIntegerDigit)
      throw new FormatException ("Input string is not in a correct format.");

   // Parse scientific notation when an exponent marker is present.
   if (index < input.Length && char.ToLower (input[index]) == 'e') {
      index++;
      bool exponentNegative = false;

      // Read the optional exponent sign.
      if (index < input.Length && input[index] is '+' or '-') {
         exponentNegative = input[index] == '-';
         index++;
      }

      int exponent = 0;
      bool hasExponentDigit = false;

      // Convert exponent digits into an integer.
      while (index < input.Length && char.IsDigit (input[index])) {
         exponent = exponent * 10 + (input[index] - '0');
         index++;
         hasExponentDigit = true;
      }

      // Reject an exponent without digits.
      if (!hasExponentDigit)
         throw new FormatException ("Invalid exponent format.");

      if (exponentNegative) exponent = -exponent;

      // Apply the exponent using a power of ten.
      result *= Math.Pow (10, exponent);
   }

   // Reject any unprocessed or invalid characters.
   if (index != input.Length)
      throw new FormatException ("Input string is not a correct format.");

   // Apply the original sign and return the parsed value.
   return isNegative ? -result : result;
}


//My program implements a custom ParseDouble method without using the built-in double.Parse ().
//It reads the input string, removes whitespace, and processes the optional sign, integer part, decimal part, and scientific notation using an index-based parsing approach.
//It converts individual characters into digits and calculates the numeric value
//using positional arithmetic. Finally, it validates the input format, applies the sign, and returns the result.
//I use FormatException to handle invalid inputs and Math.Pow() to process the exponent.”


#endregion
