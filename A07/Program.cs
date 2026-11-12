// --------------------------------------------------------------------------------------------
// Training ~ A training program for new joinee at Trumpf Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------
// Program.cs
// A09: Implementation of generic type of Queue using array.
// --------------------------------------------------------------------------------------------

using static System.Console;
using static System.ConsoleColor;

class Program {
   static void Main () {
      QueueValidator.Execute ();
   }

   class CircularQueue<T> {
      private T[] mBuffer;
      private int mFront;
      private int mRear;
      public int ItemCount { get; private set; }
      public int Capacity => mBuffer.Length;
      public bool IsEmpty => ItemCount == 0;
      public bool IsFull => ItemCount == Capacity;
      public CircularQueue (params T[] elements) {
         mBuffer = new T[4];
         foreach (T element in elements) Add (element);
      }

      // Adds an element at the rear of the queue
      public void Add (T value) {
         if (IsFull) IncreaseCapacity ();
         mBuffer[mRear] = value;
         mRear = (mRear + 1) % Capacity;
         ItemCount++;
      }

      // Removes and returns the front element
      public T Remove () {
         if (IsEmpty) throw new InvalidOperationException ("Queue does not contain any elements.");
         T result = mBuffer[mFront];
         mBuffer[mFront] = default!;
         mFront = (mFront + 1) % Capacity;
         ItemCount--;
         return result;
      }

      // Doubles the existing array capacity
      private void IncreaseCapacity () {
         int oldCapacity = Capacity;
         int newCapacity = oldCapacity * 2;
         T[] newBuffer = new T[newCapacity];
         CopyElements (newBuffer);
         mBuffer = newBuffer;
         mFront = 0;
         mRear = ItemCount;
      }

      // Copies queue elements in FIFO order
      private void CopyElements (T[] destination) {
         for (int index = 0; index < ItemCount; index++) {
            int sourceIndex = (mFront + index) % Capacity;
            destination[index] = mBuffer[sourceIndex];
         }
      }
   }

   class QueueValidator {
      public static void Execute () {
         Validate (new CircularQueue<int> (10, 20, 30, 40), 50, 10, "INTEGER");
         Validate (new CircularQueue<string> ("Ram", "Shyam", "John", "David"),
            "Peter", "Ram", "STRING");
         Validate (new CircularQueue<double> (1.5, 2.5, 3.5, 4.5), 5.5, 1.5, "DOUBLE");
      }

      private static void Validate<T> (
         CircularQueue<T> queue,
         T newValue,
         T expectedValue,
         string category) {
         WriteLine ($"\n========== {category} QUEUE ==========");
         Check (queue.ItemCount == 4, "Initial item count");
         Check (queue.IsFull, "Initial capacity reached");
         queue.Add (newValue);
         Check (queue.Capacity == 8, "Array capacity increased");
         Check (queue.ItemCount == 5, "Item count after adding");
         T removedValue = queue.Remove ();
         Check (EqualityComparer<T>.Default.Equals (removedValue, expectedValue),
            "FIFO order verification");
         Check (queue.ItemCount == 4, "Item count after removing");
         while (!queue.IsEmpty) queue.Remove ();
         Check (queue.IsEmpty, "Queue is completely empty");
         try {
            queue.Remove ();
            Check (false, "Empty queue exception");
         } catch (InvalidOperationException) {
            Check (true, "Empty queue exception");
         }
      }

      private static void Check (bool result, string description) {
         ForegroundColor = result ? Green : Red;
         WriteLine ($"{description,-32} : {(result ? "PASS" : "FAIL")}");
         ResetColor ();
      }
   }
}