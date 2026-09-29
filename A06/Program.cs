// -----------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -----------------------------------------------------------------------------------------
// A06 - Implement double.Parse method
// Program to convert a string into a double without using double.Parse().

using static System.Console;

Write ("Enter a Number: ");
string? input = ReadLine ();
try {
   double result = MyParse (input);
   WriteLine ($"Result = {result}");
} catch (Exception ex) {
   WriteLine ($"Error: {ex.Message}");
}

#region Methods ------------------------------------------------------
static double MyParse (string? input) {
   if (string.IsNullOrEmpty (input)) throw new FormatException ("Input string is null or empty.");
   int index = 0;
   bool isNegative = false;
   while (index < input.Length && input[index] == ' ') index++;
   if (index < input.Length && (input[index] == '-' || input[index] == '+')) {
      isNegative = input[index] == '-';
      index++;
   }
   double result = 0;
   bool hasDigit = false;
   while (index < input.Length && char.IsDigit (input[index])) {
      result = result * 10 + (input[index] - '0');
      index++;
      hasDigit = true;
   }
   if (index < input.Length && input[index] == ' ') {
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
   while (index < input.Length && input[index] == ' ') index++;
   if (index != input.Length) throw new FormatException ("Input string is not a correct format.");
   return isNegative ? -result : result;
}
#endregion