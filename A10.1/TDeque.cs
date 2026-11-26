
// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at TRUMPF Metamation.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// TDeque.cs
// Generic double-ended queue implemented using a circular array.
// ------------------------------------------------------------------------------------------------
using static ESide;

#region Class TDeque ------------------------------------------------------------------------------
class TDeque<T> {
   #region Properties -----------------------------------------------
   public bool IsEmpty => mUsed == 0;      // Indicates whether the queue is empty.
   public int Count => mUsed;              // Number of elements in the queue.
   int Capacity => mData.Length;           // Capacity of the internal array.
   #endregion

   #region Methods --------------------------------------------------
   public void EnqueueHead (T item) => Enqueue (item, Head);  // Adds an element at the head
   public void EnqueueTail (T item) => Enqueue (item, Tail);  // Adds an element at the tail
   public T DequeueHead () => Dequeue (Head);   // Removes and returns an element from the head.
   public T DequeueTail () => Dequeue (Tail);   // Removes and returns an element from the tail.
   // Removes all elements and resets the queue.
   public void Clear () {
      mData = new T[INITCAPACITY];
      (mHead, mTail, mUsed) = (0, 0, 0);
   }
   #endregion

   #region Implementation -------------------------------------------
   // Adds an element at the specified end.
   void Enqueue (T item, ESide side) {
      Resize ();
      if (side == Head) {
         mHead = (mHead - 1 + Capacity) % Capacity;
         mData[mHead] = item;
      } else {
         mData[mTail] = item;
         mTail = (mTail + 1) % Capacity;
      }
      mUsed++;
   }

   // Removes and returns an element from the specified end.
   T Dequeue (ESide side) {
      if (IsEmpty) throw new InvalidOperationException ("Queue is Empty");
      int index;
      if (side == Head) {
         index = mHead;
         mHead = (mHead + 1) % Capacity;
      } else {
         mTail = (mTail - 1 + Capacity) % Capacity;
         index = mTail;
      }
      T item = mData[index];
      mData[index] = default!;
      mUsed--;
      return item;
   }

   // Doubles the array capacity when it becomes full.
   void Resize () {
      if (mUsed < Capacity) return;
      T[] temp = new T[checked(Capacity * RESIZEFACTOR)];
      for (int i = 0; i < mUsed; i++) temp[i] = mData[(mHead + i) % Capacity];
      mData = temp;
      (mHead, mTail) = (0, mUsed);
   }
   #endregion

   #region Constants ------------------------------------------------
   const int INITCAPACITY = 4;
   const int RESIZEFACTOR = 2;
   #endregion

   #region Private data ---------------------------------------------
   T[] mData = new T[INITCAPACITY];
   int mHead, mTail, mUsed;
   #endregion
}
#endregion

#region Enum ESide --------------------------------------------------------------------------------
enum ESide { Head, Tail }
#endregion