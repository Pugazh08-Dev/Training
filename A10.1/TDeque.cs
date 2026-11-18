
// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Trumpf Metamation.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// TDeque.cs
// Generic double-ended queue implementation using a circular array.
// ------------------------------------------------------------------------------------------------
#region class TDeque<T> ---------------------------------------------------------------------------
class TDeque<T> {
   const int INITIAL_CAPACITY = 4;
   const int RESIZE_FACTOR = 2;
   T[] mBuffer = new T[INITIAL_CAPACITY];
   int mFront;
   int mSize;
   public int Count => mSize;
   public int Capacity => mBuffer.Length;
   public bool IsEmpty => mSize == 0;
   // Adds an item at the front
   public void AddFirst (T item) => Add (item, DequeEnd.Front);
   // Adds an item at the back
   public void AddLast (T item) => Add (item, DequeEnd.Back);
   // Removes and returns the item at the front
   public T RemoveFirst () => Remove (DequeEnd.Front);
   // Removes and returns the item at the back
   public T RemoveLast () => Remove (DequeEnd.Back);
   // Adds an item at the specified end

   #region Methods --------------------------------------------------
   void Add (T item, DequeEnd end) {
      EnsureCapacity ();
      if (end == DequeEnd.Front) {
         mFront = (mFront - 1 + Capacity) % Capacity;
         mBuffer[mFront] = item;
      } else {
         int back = (mFront + mSize) % Capacity;
         mBuffer[back] = item;
      }
      mSize++;
   }

   // Removes and returns an item from the specified end.
   T Remove (DequeEnd end) {
      if (IsEmpty) throw new InvalidOperationException ("Deque is Empty");
      int index;
      if (end == DequeEnd.Front) {
         index = mFront;
         mFront = (mFront + 1) % Capacity;
      } else index = (mFront + mSize - 1) % Capacity;
      T item = mBuffer[index];
      mBuffer[index] = default!;
      mSize--;
      if (IsEmpty) mFront = 0;
      return item;
   }

   // Ensures that the array has enough space.
   void EnsureCapacity () {
      if (mSize < Capacity) return;
      T[] newBuffer = new T[checked(Capacity * RESIZE_FACTOR)];
      for (int i = 0; i < mSize; i++) newBuffer[i] = mBuffer[(mFront + i) % Capacity];
      mBuffer = newBuffer;
      mFront = 0;
   }

   // Removes all items from the deque.
   public void Clear () {
      mBuffer = new T[INITIAL_CAPACITY];
      mFront = 0;
      mSize = 0;
   }
   #endregion
}
#endregion

#region Enum DequeEnd -----------------------------------------------------------------------------
enum DequeEnd {
   Front,
   Back
}
#endregion