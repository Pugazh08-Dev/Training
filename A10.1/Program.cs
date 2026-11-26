
// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at TRUMPF Metamation.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// Tests the generic double-ended queue implementation.
// ------------------------------------------------------------------------------------------------
using static ESide;
using static System.Console;

#region Class TestCases ---------------------------------------------------------------------------
class TestCases {
   #region Implementation -------------------------------------------
   // Runs all test cases.
   static void Main () {
      WriteLine ("DOUBLE-ENDED QUEUE TEST CASES\n----------------------------------------");
      TestEmptyThrows ();
      TestTailHeadOrder ();
      TestHeadTailOrder ();
      TestMixedOrder ();
      TestTailWrap ();
      TestHeadWrap ();
      TestTailResize ();
      TestHeadResize ();
      TestWrappedTailResize ();
      TestWrappedHeadResize ();
      TestMultipleResize ();
      TestReuse ();
      TestIsEmpty ();
      TestCount ();
      TestClear ();
      TestStringQueue ();
      WriteLine ($"----------------------------------------\nPassed: {sPass}, Failed: {sFail}");
   }

   // Tests removal from an empty queue.
   static void TestEmptyThrows () {
      bool passed = Throws (() => sQueue.DequeueHead ()) && Throws (() => sQueue.DequeueTail ());
      PrintStatus ("Empty queue exceptions", passed);
   }

   // Tests insertion at tail and removal from head.
   static void TestTailHeadOrder () {
      Fill (0, sData.Length, Tail);
      PrintStatus ("Tail to head order", Check (0, sData.Length, Head));
   }

   // Tests insertion at head and removal from tail.
   static void TestHeadTailOrder () {
      Fill (0, sData.Length, Head);
      PrintStatus ("Head to tail order", Check (0, sData.Length, Tail));
   }

   // Tests insertion and removal at both ends.
   static void TestMixedOrder () {
      sQueue.EnqueueHead (20);
      sQueue.EnqueueTail (30);
      sQueue.EnqueueHead (10);
      sQueue.EnqueueTail (40);
      bool passed = sQueue.DequeueHead () == 10 && sQueue.DequeueTail () == 40 &&
                    sQueue.DequeueHead () == 20 && sQueue.DequeueTail () == 30 && sQueue.IsEmpty;
      PrintStatus ("Mixed operations", passed);
   }

   // Tests circular wrapping at the tail.
   static void TestTailWrap () {
      Fill (0, 4, Tail);
      Check (0, 2, Head);
      Fill (4, 6, Tail);
      PrintStatus ("Tail wraparound", Check (2, 6, Head));
   }

   // Tests circular wrapping at the head.
   static void TestHeadWrap () {
      Fill (0, 4, Head);
      Check (0, 2, Tail);
      Fill (4, 6, Head);
      PrintStatus ("Head wraparound", Check (2, 6, Tail));
   }

   // Tests resizing when inserting at the tail.
   static void TestTailResize () {
      Fill (0, sData.Length, Tail);
      bool passed = sQueue.Count == sData.Length && Check (0, sData.Length, Head);
      PrintStatus ("Tail resize", passed);
   }

   // Tests resizing when inserting at the head.
   static void TestHeadResize () {
      Fill (0, sData.Length, Head);
      bool passed = sQueue.Count == sData.Length && Check (0, sData.Length, Tail);
      PrintStatus ("Head resize", passed);
   }

   // Tests resizing after the circular array wraps at the tail.
   static void TestWrappedTailResize () {
      Fill (0, 4, Tail);
      Check (0, 2, Head);
      Fill (4, sData.Length, Tail);
      bool passed = sQueue.Count == sData.Length - 2 && Check (2, sData.Length, Head);
      PrintStatus ("Wrapped tail resize", passed);
   }

   // Tests resizing after the circular array wraps at the head.
   static void TestWrappedHeadResize () {
      Fill (0, 4, Tail);
      Check (0, 2, Head);
      Fill (4, 6, Tail);
      sQueue.EnqueueHead (-10);
      bool passed = sQueue.DequeueHead () == -10 && Check (2, 6, Head);
      PrintStatus ("Wrapped head resize", passed);
   }

   // Tests multiple capacity expansions.
   static void TestMultipleResize () {
      const int COUNT = 20;
      for (int i = 0; i < COUNT; i++) sQueue.EnqueueTail (i);
      bool passed = sQueue.Count == COUNT;
      for (int i = 0; i < COUNT; i++) passed &= sQueue.DequeueHead () == i;
      PrintStatus ("Multiple resize", passed && sQueue.IsEmpty);
   }

   // Tests reusing the queue after removing all elements.
   static void TestReuse () {
      Fill (0, 3, Tail);
      Check (0, 3, Head);
      Fill (0, sData.Length, Tail);
      bool passed = Check (0, sData.Length, Head);
      PrintStatus ("Queue reuse", passed);
   }

   // Tests the IsEmpty property.
   static void TestIsEmpty () {
      bool passed = sQueue.IsEmpty;
      sQueue.EnqueueHead (10);
      passed &= sQueue.IsEmpty;     // Incorrect: should be false
      sQueue.DequeueTail ();
      passed &= sQueue.IsEmpty;
      PrintStatus ("IsEmpty property", passed);
   }

   // Tests the Count property.
   static void TestCount () {
      bool passed = sQueue.Count == 0;
      sQueue.EnqueueHead (10);
      sQueue.EnqueueTail (20);
      passed &= sQueue.Count == 5;   // Incorrect: should be 2
      sQueue.DequeueHead ();
      passed &= sQueue.Count == 1;
      sQueue.DequeueTail ();
      passed &= sQueue.Count == 0;
      PrintStatus ("Count property", passed);
   }

   // Tests clearing the queue.
   static void TestClear () {
      Fill (0, sData.Length, Tail);
      sQueue.Clear ();
      bool passed = sQueue.IsEmpty && sQueue.Count == 0;
      sQueue.EnqueueHead (100);
      passed &= sQueue.DequeueTail () == 100;
      PrintStatus ("Clear operation", passed);
   }

   // Tests the queue with string values.
   static void TestStringQueue () {
      TDeque<string> queue = new ();
      queue.EnqueueTail ("C#");
      queue.EnqueueHead ("DotNet");
      queue.EnqueueTail ("WPF");
      bool passed = queue.DequeueHead () == "DotNet" && queue.DequeueTail () == "WPF" &&
                    queue.DequeueHead () == "C#" && queue.IsEmpty;
      PrintStatus ("String queue", passed);
   }
   #endregion

   #region Helpers --------------------------------------------------
   // Inserts test values at the specified end.
   static void Fill (int from, int to, ESide side) {
      for (int i = from; i < to; i++) {
         if (side == Head) sQueue.EnqueueHead (sData[i]);
         else sQueue.EnqueueTail (sData[i]);
      }
   }

   // Removes and verifies test values from the specified end.
   static bool Check (int from, int to, ESide side) {
      bool passed = true;
      for (int i = from; i < to; i++) {
         int actual = side == Head ? sQueue.DequeueHead () : sQueue.DequeueTail ();
         passed &= actual == sData[i];
      }
      return passed;
   }

   // Checks whether an operation throws InvalidOperationException.
   static bool Throws (Action action) {
      try {
         action ();
      } catch (InvalidOperationException) {
         return true;
      }
      return false;
   }

   // Displays the test result.
   static void PrintStatus (string name, bool passed) {
      if (passed) sPass++; else sFail++;
      Write ("[");
      ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
      Write (passed ? "PASS" : "FAIL");
      ResetColor ();
      WriteLine ($"] {name}");
      sQueue.Clear ();
   }
   #endregion

   #region Private data ---------------------------------------------
   static readonly int[] sData = { 0, 10, 20, 30, 40, 50, 60 };
   static readonly TDeque<int> sQueue = new ();
   static int sPass, sFail;
   #endregion
}
#endregion