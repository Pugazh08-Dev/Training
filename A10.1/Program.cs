// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Trumpf Metamation.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Exercise: Implement file name parser using a state machine.
// ------------------------------------------------------------------------------------------------

using System.Text;
using static System.Console;

#region Enum : ParserState ------------------------------------------
enum ParserState {
   DriveLetter,
   Colon,
   RootSeperator,
   PathSegment,
   Extension,
   Complete,
   Error
}
#endregion

#region Class : FileNameParser --------------------------------------
static class FilenNameParser {
   public static (string Drive, string Directory, string FileName, string Extension)
      Parse (string path) {
      if (!TryParse (path, out var result, out string error, new ()))
         throw new FormatException (error);
      return result;
   }
   public static bool TryParse (string path, out (string Drive, string Directory, string FileName,
      string Extension) result, out string error, StringBuilder segment) {
      ArgumentNullException.ThrowIfNull (segment);
      result = default;
      error = string.Empty;
      if (path == null) {
         error = "Path cannot be null.";
         return false;
      }
      if (path.Length == 0) {
         error = "Path cannot be empty.";
         return false;
      }
      ParserState state = ParserState.DriveLetter;
      string drive = string.Empty;
      string filename = string.Empty;
      string extension = string.Empty;
      StringBuilder directory = new ();
      int directoryCount = 0;
      int pos = 0;
      while (pos < path.Length) {
         char current = path[pos];
         switch (state) {
            // ------------------------------------------------------
            case ParserState.DriveLetter:
               if (IsValidDrive (current)) {
                  drive = current.ToString ();
                  state = ParserState.Colon;
               } else {
                  error = $"Drive letter must be between A and Z.";
                  return false;
               }
               break;
            // ------------------------------------------------------
            case ParserState.Colon:
               if (current == ':') state = ParserState.RootSeperator;
               else {
                  error = $"Missing colon after drive letter at pos {pos}.";
                  return false;
               }
               break;
            // ------------------------------------------------------
            case ParserState.RootSeperator:
               if (current == '\\') state = ParserState.PathSegment;
               else {
                  error = $"Missing root seperator '\\' after colon at pos {pos}.";
                  return false;
               }
               break;
            // ------------------------------------------------------
            case ParserState.PathSegment:
               if (IsValidCharacter (current)) segment.Append (current);
               else if (current == '\\') {
                  if (segment.Length == 0) {
                     error = $"Empty directory name detected at pos {pos}.";
                     return false;
                  }
                  if (directory.Length > 0) directory.Append ('\\');
                  directory.Append (segment);
                  directoryCount++;
                  segment.Clear ();
               } else if (current == '.') {
                  if (segment.Length == 0) {
                     error = $"File name is missing before the extension at pos {pos}.";
                     return false;
                  }
                  if (directoryCount == 0) {
                     error = $"Atleast one directory is required before the filename";
                     return false;
                  }
                  filename = segment.ToString ();
                  segment.Clear ();
                  state = ParserState.Extension;
               } else if (char.IsLower (current)) {
                  error = $"Lowercase character '{current}' is not allowed at pos {pos}.";
                  return false;
               } else {
                  error = $"Invalid character '{current}' at pos {pos}.";
                  return false;
               }
               break;
            // ------------------------------------------------------
            case ParserState.Extension:
               if (IsValidCharacter (current)) extension += current;
               else if (char.IsLower (current)) {
                  error = $"Lowercase character '{current}' is not allowed at pos {pos}.";
                  return false;
               } else {
                  error = $"Invalid character '{current}' at pos {pos}.";
                  return false;
               }
               break;
         }
         pos++;
      }
      // Final state validation
      if (state == ParserState.PathSegment) {
         if (segment.Length == 0) error = $"File name is missing.";
         else if (directoryCount == 0)
            error = $"At Least one directory and a file name with extension are required.";
         else error = $"Missing file extension";
         return false;
      }
      if (state == ParserState.Extension && extension.Length == 0) {
         error = $"File extension cannot be empty";
         return false;
      }
      if (state != ParserState.Extension) {
         error = $"Incomplete filepath";
         return false;
      }
      state = ParserState.Complete;
      result = (drive, directory.ToString (), filename, extension);
      return true;
   }
   #endregion

   #region Validation Methods ---------------------------------------
   /// Checks whether the character is valid.
   private static bool IsValidCharacter (char value) => (value is >= 'A' and <= 'Z') ||
      (value is >= '0' and <= '9');
   /// Checks whether the character belongs to A-Z.
   private static bool IsValidDrive (char value) => value is >= 'A' and <= 'Z';
   #endregion
}

#region Class : Program ---------------------------------------------
class Program {
   #region Main -----------------------------------------------------
   static void Main () {
      string[] testCases = {
         @"C:\FOLDER1\FOLDER2\SAMPLE.TXT",
         @"D:\PROJECT\SOURCE\PROGRAM.CS",
         @"A:\DATA\REPORT.PDF",
         @"Z:\FOLDER\FILE",
         @"C:\FOLDER1\FOLDER2\FILE.",
         @"C:\FOLDER1\\FILE.TXT",
         @"C:\folder\FILE.TXT",
         @"1:\FOLDER\FILE.TXT",
         @"C:\FILE.TXT",
         @"C:FOLDER\FILE.TXT"
      };
      var validPaths = new List<string> ();
      var invalidPaths = new List<(string Path, string Reason)> ();
      // Parse all input paths.
      foreach (string path in testCases) {
         StringBuilder segment = new ();
         if (FilenNameParser.TryParse (path, out var result, out string error, segment))
            validPaths.Add (path);
         else invalidPaths.Add ((path, error));
      }
      // Display valid paths.
      ForegroundColor = ConsoleColor.Green;
      WriteLine ($"\n============ VALID FILE PATHS ============\n");
      ResetColor ();
      foreach (string path in validPaths) {
         var result = FilenNameParser.Parse (path);
         ForegroundColor = ConsoleColor.Green;
         WriteLine ($"[VALID] {path}");
         ResetColor ();
         WriteLine ($"   Drive     : {result.Drive} \n   Directory : {result.Directory} \n" +
            $"   File Name : {result.FileName} \n   Extension : {result.Extension}\n");
      }
      // Display invalid paths.
      ForegroundColor = ConsoleColor.Red;
      WriteLine ($"\n============ INVALID FILE PATHS ============\n");
      ResetColor ();
      foreach (var item in invalidPaths) {
         ForegroundColor = ConsoleColor.Red;
         WriteLine ($"[INVALID] {item.Path}");
         ResetColor ();
         WriteLine ($"   Reason : {item.Reason}\n");
      }
      // Summary.
      WriteLine ($"--------------------------------------- \nTotal Paths   : {testCases.Length}");
      ForegroundColor = ConsoleColor.Green;
      WriteLine ($"Valid Paths   : {validPaths.Count}");
      ResetColor ();
      ForegroundColor = ConsoleColor.Red;
      WriteLine ($"Invalid Paths : {invalidPaths.Count}");
      ResetColor ();
      WriteLine ("---------------------------------------");
      ReadKey ();
   }
   #endregion
}
#endregion