// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// A10.1 - Generic Double-Ended Queue implementation using a circular array.
// ------------------------------------------------------------------------------------------------

using System.Collections;
using static System.Console;

#region class program -----------------------------------------------------------------------------
class Program {
   static void Main () {
      TDeque<int> deque = new TDeque<int> ();
      // Adding elements at both ends
      deque.AddLast (10);
      deque.AddLast (20);
      deque.AddFirst (5);
      deque.AddFirst (1);
      WriteLine ("Initial Deque:");
      Display (deque);
      // Checking first and last elements
      WriteLine ("\nFirst Element: " + deque.PeekFirst ());
      WriteLine ("Last Element: " + deque.PeekLast ());
      WriteLine ("\nRemoved First: " + deque.RemoveFirst ());
      WriteLine ("Removed Last: " + deque.RemoveLast ());
      WriteLine ("\nDeque After Removals:");
      Display (deque);
      // Adding more elements
      deque.AddLast (30);
      deque.AddFirst (2);
      WriteLine ("\nDeque after adding more elements:");
      Display (deque);
      // Searching
      WriteLine ("\nContains 20: " + deque.Contains (20));
      WriteLine ("Contains 50: " + deque.Contains (50));
      // Capacity and count
      WriteLine ("\nCount : " + deque.Count);
      WriteLine ("Capacity: " + deque.Capacity);
      // Converting to array
      int[] result = [.. deque];
      WriteLine ("\nDeque as Array");
      WriteLine (string.Join (",", result));
   }

   static void Display<T> (TDeque<T> deque) => WriteLine ("[" + string.Join (", ", deque) + "]");
}
#endregion

#region class -------------------------------------------------------------------------------------
public class TDeque<T> : IEnumerable<T> {
   #region Fields ---------------------------------------------------
   T[] mData;
   int mHead;
   int mCount;
   const int DefaultCapacity = 4;
   #endregion

   #region Properties -----------------------------------------------
   public int Count => mCount;
   public int Capacity => mData.Length;
   public bool IsEmpty => mCount == 0;
   #endregion

   #region Constructor ----------------------------------------------
   public TDeque (int capacity = DefaultCapacity) {
      ArgumentOutOfRangeException.ThrowIfLessThan (capacity, 1);
      mData = new T[capacity];
      mHead = 0;
      mCount = 0;
   }
   #endregion

   #region Methods --------------------------------------------------
   // Adds an element at the beginning.
   public void AddFirst (T item) {
      EnsureCapacity ();
      mHead = (mHead - 1 + Capacity) % Capacity;
      mData[mHead] = item;
      mCount++;
   }

   // Adds an element at the end
   public void AddLast (T item) {
      EnsureCapacity ();
      int tail = (mHead + mCount) % Capacity;
      mData[tail] = item;
      mCount++;
   }

   // Removes and returns the first element
   public T RemoveFirst () {
      EnsureNotEmpty ();
      T item = mData[mHead];
#pragma warning disable CS8601 // Possible null reference assignment.
      mData[mHead] = default;
#pragma warning restore CS8601 // Possible null reference assignment.
      mHead = (mHead + 1) % Capacity;
      mCount--;
      ResetHeadIfEmpty ();
      return item;
   }

   // Removes and returns the last element
   public T RemoveLast () {
      EnsureNotEmpty ();
      int tail = (mHead + mCount - 1) % Capacity;
      T item = mData[tail];
#pragma warning disable CS8601 // Possible null reference assignment.
      mData[tail] = default;
#pragma warning restore CS8601 // Possible null reference assignment.
      mCount--;
      ResetHeadIfEmpty ();
      return item;
   }

   // Returns the first element without removing it
   public T PeekFirst () {
      EnsureNotEmpty ();
      return mData[mHead];
   }

   // Returns the last element without removing it
   public T PeekLast () {
      EnsureNotEmpty ();
      int tail = (mHead + mCount - 1) % Capacity;
      return mData[tail];
   }

   // Search Operations
   public bool Contains (T item) {
      EqualityComparer<T> comparer = EqualityComparer<T>.Default;
      for (int i = 0; i < mCount; i++) {
         int index = (mHead + i) % Capacity;
         if (comparer.Equals (mData[index], item)) return true;
      }
      return false;
   }

   // Utility Operations
   public void clear () {
      Array.Clear (mData, 0, Capacity);
      mHead = 0;
      mCount = 0;
   }

   public T[] ToArray () {
      T[] result = new T[mCount];
      for (int i = 0; i < mCount; i++) result[i] = mData[(mHead + i) % Capacity];
      return result;
   }

   void EnsureCapacity () {
      if (mCount < Capacity) return;
      int newCapacity = checked(Capacity * 2);
      T[] newData = new T[newCapacity];
      for (int i = 0; i < mCount; i++) newData[i] = mData[(mHead + i) % Capacity];
      mData = newData;
      mHead = 0;
   }

   void EnsureNotEmpty () {
      if (IsEmpty)
         throw new InvalidOperationException ("Cannot remove or access element from empty deque.");
   }

   void ResetHeadIfEmpty () {
      if (IsEmpty) mHead = 0;
   }
   #endregion

   #region Enumeration ----------------------------------------------
   public IEnumerator<T> GetEnumerator () {
      for (int i = 0; i < mCount; i++) yield return mData[(mHead + i) % Capacity];
   }

   IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
   #endregion
}
#endregion