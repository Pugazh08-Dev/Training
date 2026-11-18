
// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Trumpf Metamation.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Program to test the generic double-ended queue implementation.
// ------------------------------------------------------------------------------------------------
using static System.Console;

#region class Program -----------------------------------------------------------------------------
class Program {
   static void Main () => new TestCases().RunAll ();
}
#endregion

#region class TestCases ----------------------------------------------------------------------------
class TestCases {
   #region Method RunAll --------------------------------------------
   TDeque<int> deque = new();
   public void RunAll () {
      WriteLine ("DOUBLE-ENDED QUEUE TEST CASES \n----------------------------------------");
      TestEmptyThrows ();
      TestBackToFrontOrder ();
      TestFrontToBackOrder ();
      TestMixedOrder ();
      TestBackWrap ();
      TestFrontWrap ();
      TestBackResize ();
      TestFrontResize ();
      TestWrappedBackResize ();
      TestWrappedFrontResize ();
      TestMultipleResize ();
      TestReuse ();
      TestIsEmpty ();
      TestCount ();
      TestClear ();
      TestStringDeque ();
      WriteLine ($"---------------------------------------- \nTotal Passed : {sPassed}" +
         $"\nTotal Failed : {sFailed}");
   }
   #endregion

   #region Methods TestCases ----------------------------------------
   // Verifies that removing from an empty deque throws an exception.
   void TestEmptyThrows () {
      bool first = ThrowsException (() => deque.RemoveFirst ());
      bool last = ThrowsException (() => deque.RemoveLast ());
      PrintStatus ("Empty deque exception", first && last);
   }

   // Adds at the back and removes from the front.
   void TestBackToFrontOrder () {
      Fill (deque, 0, 5, DequeEnd.Back);
      bool result = Check (deque, 0, 5, DequeEnd.Front);
      PrintStatus ("Back to front order", result);
   }

   // Adds at the front and removes from the back.
   void TestFrontToBackOrder () {
      Fill (deque, 0, 5, DequeEnd.Front);
      bool result = Check (deque, 0, 5, DequeEnd.Back);
      PrintStatus ("Front to back order", result);
   }

   // Verifies mixed front and back operations.
   void TestMixedOrder () {
      deque.AddFirst (20);
      deque.AddLast (30);
      deque.AddFirst (10);
      deque.AddLast (40);
      bool result = deque.RemoveFirst () == 10 && deque.RemoveLast () == 40 &&
         deque.RemoveFirst () == 20 && deque.RemoveLast () == 30 && deque.IsEmpty;
      PrintStatus ("Mixed front and back operations", result);
   }

   // Verifies circular array wraparound when adding at the back.
   void TestBackWrap () {
      Fill (deque, 0, 4, DequeEnd.Back);
      Check (deque, 0, 2, DequeEnd.Front);
      deque.AddLast (40);
      deque.AddLast (50);
      bool result = Check (deque, 2, 6, DequeEnd.Front);
      PrintStatus ("Back wraparound", result);
   }

   // Verifies circular array wraparound when adding at the front.
   void TestFrontWrap () {
      Fill (deque, 0, 4, DequeEnd.Back);
      Check (deque, 0, 2, DequeEnd.Front);
      deque.AddFirst (-10);
      deque.AddFirst (-20);
      bool result = deque.RemoveFirst () == -20 && deque.RemoveFirst () == -10 &&
         Check (deque, 2, 4, DequeEnd.Front);
      PrintStatus ("Front wraparound", result);
   }

   // Verifies resizing when adding at the back.
   void TestBackResize () {
      Fill (deque, 0, 7, DequeEnd.Back);
      bool result = deque.Capacity >= 8 && Check (deque, 0, 7, DequeEnd.Front);
      PrintStatus ("Back resize", result);
   }

   // Verifies resizing when adding at the front.
   void TestFrontResize () {
      Fill (deque, 0, 7, DequeEnd.Front);
      bool result = deque.Capacity >= 8 && Check (deque, 0, 7, DequeEnd.Back);
      PrintStatus ("Front resize", result);
   }

   // Verifies resizing when the circular array is wrapped.
   void TestWrappedBackResize () {
      Fill (deque, 0, 4, DequeEnd.Back);
      Check (deque, 0, 2, DequeEnd.Front);
      deque.AddLast (40);
      deque.AddLast (50);
      deque.AddLast (60);
      bool result = deque.Capacity >= 8 && deque.Count == 5 && deque.RemoveFirst () == 20 &&
         deque.RemoveFirst () == 30 && deque.RemoveFirst () == 40 && deque.RemoveFirst () == 50 &&
         deque.RemoveFirst () == 60;
      PrintStatus ("Wrapped back resize", result);
   }

   // Verifies resizing when adding at the front of a wrapped array.
   void TestWrappedFrontResize () {
      Fill (deque, 0, 4, DequeEnd.Back);
      Check (deque, 0, 2, DequeEnd.Front);
      deque.AddLast (40);
      deque.AddLast (50);
      deque.AddFirst (-10);
      bool result = deque.Capacity >= 8 && deque.RemoveFirst () == -10 &&
         Check (deque, 2, 6, DequeEnd.Front);
      PrintStatus ("Wrapped front resize", result);
   }

   // Verifies multiple resizing operations.
   void TestMultipleResize () {
      for (int i = 0; i < 20; i++) deque.AddLast (i);
      bool result = deque.Capacity >= 20 && deque.Count == 20;
      for (int i = 0; i < 20; i++) result &= deque.RemoveFirst () == i;
      PrintStatus ("Multiple resize", result && deque.IsEmpty);
   }

   // Verifies that the deque can be reused after becoming empty.
   void TestReuse () {
      deque.AddLast (10);
      deque.RemoveFirst ();
      deque.AddFirst (20);
      deque.AddLast (30);
      bool result = deque.RemoveFirst () == 20 && deque.RemoveLast () == 30 && deque.IsEmpty;
      PrintStatus ("Reuse after empty", result);
   }

   // Verifies the IsEmpty property.
   void TestIsEmpty () {
      bool initiallyEmpty = deque.IsEmpty;
      deque.AddLast (10);
      bool notEmpty = !deque.IsEmpty;
      deque.RemoveFirst ();
      bool emptyAgain = deque.IsEmpty;
      PrintStatus ("IsEmpty property", initiallyEmpty && notEmpty && emptyAgain);
   }

   // Verifies the Count property.
   void TestCount () {
      bool result = deque.Count == 0;
      deque.AddFirst (10);
      deque.AddLast (20);
      result &= deque.Count == 2;
      deque.RemoveFirst ();
      result &= deque.Count == 1;
      deque.RemoveLast ();
      result &= deque.Count == 0;
      PrintStatus ("Count property", result);
   }

   // Verifies Clear operation.
   void TestClear () {
      Fill (deque, 0, 7, DequeEnd.Back);
      deque.Clear ();
      bool result = deque.IsEmpty && deque.Count == 0 && deque.Capacity == 4;
      deque.AddFirst (100);
      result &= deque.RemoveLast () == 100;
      PrintStatus ("Clear operation", result);
   }

   // Verifies that the deque works with string values.
   void TestStringDeque () {
      TDeque<string> deque = new ();
      deque.AddLast ("C#");
      deque.AddFirst ("DotNet");
      deque.AddLast ("WPF");
      bool result = deque.RemoveFirst () == "DotNet" && deque.RemoveLast () == "WPF" &&
         deque.RemoveFirst () == "C#" && deque.IsEmpty;
      PrintStatus ("String deque", result);
   }

   // Adds test data to the specified end.
   void Fill (TDeque<int> deque, int from, int to, DequeEnd end) {
      for (int i = from; i < to; i++) {
         if (end == DequeEnd.Front) deque.AddFirst (sData[i]);
         else deque.AddLast (sData[i]);
      }
   }

   // Removes and verifies test data from the specified end.
   bool Check (TDeque<int> deque, int from, int to, DequeEnd end) {
      bool result = true;
      for (int i = from; i < to; i++) {
         int actual = end == DequeEnd.Front ? deque.RemoveFirst () : deque.RemoveLast ();
         result &= actual == sData[i];
      }
      return result;
   }

   // Checks whether an operation throws InvalidOperationException.
   bool ThrowsException (Action action) {
      try {
         action ();
         return false;
      } catch (InvalidOperationException) {
         return true;
      }
   }

   // Displays the result of each test.
   void PrintStatus (string testName, bool passed) {
      if (passed) {
         WriteLine ($"[PASS] {testName}");
         sPassed++;
      } else {
         WriteLine ($"[FAIL] {testName}");
         sFailed++;
      }
   }
   #endregion

   #region Fields ---------------------------------------------------
   static int sPassed = 0;
   static int sFailed = 0;
   static readonly int[] sData = { 10, 20, 30, 40, 50, 60, 70, 80 };
   #endregion
}
#endregion
