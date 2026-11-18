
// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Trumpf Metamation.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to implement file name parser using a state machine.
// ------------------------------------------------------------------------------------------------

using System.Text;
using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   static void Main () {
      #region Private data ------------------------------------------
      string[] testCases = [
         @"C:\FOLDER\DATA\SAMPLE.TXT", @"D:\PROJECT\SOURCE\PROGRAM.CS", @"A:\DATA\REPORT.PDF",
         @"Z:\FOLDER\FILE", @"C:\FOLDER\DATA\FILE.", @"C:\FOLDER\\FILE.TXT", @"C:\folder\FILE.TXT",
         @"Cz:\FOLDER\FILE.TXT", @"C:\FILE.TXT", @"C:FOLDER\FILE.TXT"
      ];
      #endregion
      int validCount = 0;
      int invalidCount = 0;
      WriteLine ("FILE NAME PARSER \n----------------");
      foreach (string path in testCases) {
         if (FileNameParser.TryParse (path, out var result, out string error)) {
            ForegroundColor = ConsoleColor.Green;
            WriteLine ($"\nVALID   : {path}");
            ResetColor ();
            WriteLine ($"Drive     : {result.Drive}\nDirectory : {result.Directory}" +
               $"\nFile Name : {result.FileName}\nExtension : {result.Extension}");
            validCount++;
         } else {
            ForegroundColor = ConsoleColor.Red;
            WriteLine ($"\nINVALID : {path}");
            ResetColor ();
            WriteLine ($"Reason    : {error}");
            invalidCount++;
         }
      }
      WriteLine ($"\n----------------\nValid Paths   : {validCount}" +
         $"\nInvalid Paths : {invalidCount}");
   }
}
#endregion

#region class FileNameParser ----------------------------------------------------------------------
class FileNameParser {
   public static (string Drive, string Directory, string FileName, string Extension)
      Parse (string path) {
      if (TryParse (path, out var result, out string error)) return result;
      throw new FormatException (error);
   }

   #region Method TryParse ------------------------------------------
   public static bool TryParse (string path,
      out (string Drive, string Directory, string FileName, string Extension) result,
      out string error) {
      result = ("", "", "", "");
      error = "";
      if (string.IsNullOrWhiteSpace (path)) {
         error = "Path cannot be empty.";
         return false;
      }
      ParserState state = ParserState.DriveLetter;
      static void none () { }
      Action todo;
      string drive = "";
      StringBuilder directory = new ();
      StringBuilder segment = new ();
      string fileName = "";
      StringBuilder extension = new ();
      int directoryCount = 0;
      int position = 0;
      string parseError = "";
      foreach (char ch in path) {
         // Explicit tuple type fixes CS8131 and CS8506.
         (ParserState nextState, Action nextAction) transition = (state, ch) switch {
            (ParserState.DriveLetter, >= 'A' and <= 'Z') =>
               (ParserState.Colon, () => drive = ch.ToString ()),
            (ParserState.Colon, ':') => (ParserState.RootSeparator, none),
            (ParserState.RootSeparator, '\\') => (ParserState.PathSegment, none),
            (ParserState.PathSegment, >= 'A' and <= 'Z') =>
               (ParserState.PathSegment, () => segment.Append (ch)),
            (ParserState.PathSegment, '\\') when segment.Length > 0 =>
               (ParserState.PathSegment, () => {
                  if (directory.Length > 0) directory.Append ('\\');
                  directory.Append (segment);
                  directoryCount++;
                  segment.Clear ();
               }
            ),
            (ParserState.PathSegment, '.') when segment.Length > 0 && directoryCount > 0 =>
                  (ParserState.Extension, () => {
                     fileName = segment.ToString ();
                     segment.Clear ();
                  }
            ),
            (ParserState.Extension, >= 'A' and <= 'Z') =>
               (ParserState.Extension, () => extension.Append (ch)),
            _ => (ParserState.Error, () => {
               parseError = GetErrorMessage (state, ch, segment, directoryCount, position);
            }
            )
         };
         state = transition.nextState;
         todo = transition.nextAction;
         todo ();
         if (state == ParserState.Error) {
            error = parseError;
            return false;
         }
         position++;
      }
      // Validate the final parser state.
      if (state == ParserState.PathSegment) {
         if (segment.Length == 0 && directoryCount == 0)
            error = "At least one directory and a file name are required.";
         else if (segment.Length == 0) error = "File name is missing.";
         else if (directoryCount == 0) error = "At least one directory is required.";
         else error = "File extension is missing.";
         return false;
      }
      if (state == ParserState.Extension && extension.Length == 0) {
         error = "File extension cannot be empty.";
         return false;
      }
      if (state != ParserState.Extension) {
         error = "Incomplete file path.";
         return false;
      }
      state = ParserState.Complete;
      result = (drive, directory.ToString (), fileName, extension.ToString ());
      return true;
   }
   #endregion

   #region Method GetErrorMessage -----------------------------------
   static string GetErrorMessage (ParserState state, char ch, StringBuilder segment,
      int directoryCount, int position) => state switch {
         ParserState.DriveLetter => "Drive letter must be an uppercase letter between A and Z.",
         ParserState.Colon => "Colon ':' expected after the drive letter.",
         ParserState.RootSeparator => "Root separator '\\' expected after the colon.",
         ParserState.PathSegment when ch == '\\' && segment.Length == 0 =>
            "Empty directory names are not allowed.",
         ParserState.PathSegment when ch == '.' && segment.Length == 0 => "File name is missing.",
         ParserState.PathSegment when ch == '.' && directoryCount == 0 =>
            "At least one directory is required before the file name.",
         ParserState.PathSegment when ch >= 'a' && ch <= 'z' =>
            "Only uppercase letters A-Z are allowed in directory and file names.",
         ParserState.Extension when ch >= 'a' && ch <= 'z' =>
            "Only uppercase letters A-Z are allowed in the extension.",
         ParserState.PathSegment => $"Invalid character '{ch}' at position {position}.",
         ParserState.Extension => $"Invalid character '{ch}' in file extension.",
         _ => "Invalid file path format."
      };
   #endregion

   #region Enum ParserState -----------------------------------------------------------------------
   enum ParserState {
      DriveLetter,
      Colon,
      RootSeparator,
      PathSegment,
      Extension,
      Complete,
      Error
   }
   #endregion
}
#endregion
