// -----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -----------------------------------------------------------------------------------------
// A07 - Implement double.Parse method
// Program to convert a string into a double without using double.Parse().

using static System.Console;

Write ("Enter a Number: ");
string? input = ReadLine ();
try {
   double result = ParseDouble (input);
   WriteLine ($"Result = {result}");
} catch (Exception ex) {
   WriteLine ($"Error: {ex.Message}");
}

#region Methods ------------------------------------------------------
static double ParseDouble (string? input) {
   if (string.IsNullOrEmpty (input)) throw new FormatException ("Input string is null or empty.");
   input = input.Trim ();
   int index = 0;
   bool isNegative = false;
   while (index < input.Length && input[index] == ' ') index++;
   if (index < input.Length && (input[index] == '-' || input[index] == '+')) {
      isNegative = input[index] == '-';
      index++;
   }
   double result = 0;
   bool hasDigit = false;
   // Read integer part
   while (index < input.Length && char.IsDigit (input[index])) {
      result = result * 10 + (input[index] - '0');
      index++;
      hasDigit = true;
   }
   // Read decimal part
   if (index < input.Length && input[index] == '.') {
      index++;
      double decimalPlace = 0.1;
      while (index < input.Length && char.IsDigit (input[index])) {
         result += (input[index] - '0') * decimalPlace;
         decimalPlace *= 0.1;
         index++;
         hasDigit = true;
      }
   }
   if (!hasDigit) throw new FormatException ("Input string is not in a correct format.");
   // Read exponent part
   if (index < input.Length && (input[index] == 'e' || input[index] == 'E')) {
      index++;
      bool exponentNegative = false;
      // Exponent sign
      if (index < input.Length && (input[index] == '+' || input[index] == '-')) {
         exponentNegative = input[index] == '-';
         index++;
      }
      int exponent = 0;
      bool hasExponentDigit = false;
      // Read exponent digits
      while (index < input.Length && char.IsDigit (input[index])) {
         exponent = exponent * 10 + (input[index] - '0');
         index++;
         hasExponentDigit = true;
      }
      // e/E must be followed by at least one digit
      if (!hasExponentDigit) throw new FormatException ("Invalid exponent format.");
      // Apply exponent
      if (exponentNegative) exponent = -exponent;
      result *= Math.Pow (10, exponent);
   }
   while (index < input.Length && input[index] == ' ') index++;
   if (index != input.Length) throw new FormatException ("Input string is not a correct format.");
   return isNegative ? -result : result;
}
#endregion