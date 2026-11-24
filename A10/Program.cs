
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
      int valCount = 0, invalCount = 0;
      WriteLine ("FILE NAME PARSER\n----------------");
      foreach (string path in sTestCases) {
         if (FileNameParser.TryParse (path, out var result)) {
            ForegroundColor = ConsoleColor.Green;
            WriteLine ($"\nVALID   : {path}");
            ResetColor ();
            WriteLine ($"Drive     : {result.Drive} \nDirectory : {result.Directory}" +
               $"\nFile Name : {result.FileName} \nExtension : {result.Extension}");
            valCount++;
         } else {
            ForegroundColor = ConsoleColor.Red;
            WriteLine ($"\nINVALID : {path} \nInvalid file format.");
            ResetColor ();
            invalCount++;
         }
      }
      WriteLine ($"\n----------------\nValid Paths   : {valCount}\nInvalid Paths : {invalCount}");
   }

   #region Private data ---------------------------------------------
   static string[] sTestCases = [
         @"C:\FOLDER\DATA\SAMPLE.TXT", @"D:\PROJECT\SOURCE\PROGRAM.CS", @"A:\DATA\REPORT.PDF",
         @"Z:\FOLDER\FILE", @"C:\FOLDER\DATA\FILE.", @"C:\FOLDER\\FILE.TXT", @"C:\folder\FILE.TXT",
         @"Cz:\FOLDER\FILE.TXT", @"C:\FILE.TXT", @"C:FOLDER\FILE.TXT", @"C:\FOLDER1\FILE.TXT",
         @"C:\FOLDER\FILE123.TXT"
      ];
   #endregion
}
#endregion

#region class FileNameParser ----------------------------------------------------------------------
class FileNameParser {
   #region Methods --------------------------------------------------
   public static bool TryParse (string path,
      out (string Drive, string Directory, string FileName, string Extension) result) {
      result = ("", "", "", "");
      if (string.IsNullOrWhiteSpace (path)) return false;
      State state = State.DriveLetter;
      Action none = () => { }, todo;
      string drive = "", fileName = "";
      StringBuilder directory = new (), segmt = new (), extension = new ();
      int directoryCount = 0;
      foreach (char ch in path) {
         (state, todo) = (state, ch) switch {
            (State.DriveLetter, >= 'A' and <= 'Z') => (State.Colon, () => drive = ch.ToString ()),
            (State.Colon, ':') => (State.RootSeparator, none),
            (State.RootSeparator, '\\') => (State.PathSegment, none),
            (State.PathSegment, >= 'A' and <= 'Z') => (State.PathSegment, () => segmt.Append (ch)),
            (State.PathSegment, '\\') when segmt.Length > 0 => (State.PathSegment, () => {
               if (directory.Length > 0) directory.Append ('\\');
               directory.Append (segmt);
               directoryCount++;
               segmt.Clear ();
            }
            ),
            (State.PathSegment, '.') when segmt.Length > 0 && directoryCount > 0 =>
               (State.Extension, () => {
                  fileName = segmt.ToString ();
                  segmt.Clear ();
               }
            ),
            (State.Extension, >= 'A' and <= 'Z') => (State.Extension, () => extension.Append (ch)),
            _ => (State.Error, none)
         };
         todo ();
         if (state == State.Error) return false;
      }
      if (state != State.Extension || segmt.Length > 0 || extension.Length == 0) return false;
      state = State.Complete;
      result = (drive, directory.ToString (), fileName, extension.ToString ());
      return true;
   }
   #endregion
}
#endregion

#region Enum State --------------------------------------------------------------------------------
enum State {
   DriveLetter, Colon, RootSeparator, PathSegment, Extension, Complete, Error
}
#endregion