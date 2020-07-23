using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StringListPerf
{
    /// <summary>
    /// Simple segmented list
    /// </summary>
    public class SimpleSegmentedList<T> : IReadOnlyList<T>, IList
    {
        private const int SegmentSize = 256;

        private int segmentCount;
        private int count;
        private T[][] segments;

        public SimpleSegmentedList()
        {
        }

        public T this[int index]
        {
            get
            {
                return this.segments[index / SimpleSegmentedList<T>.SegmentSize][index % SimpleSegmentedList<T>.SegmentSize];
            }

            set
            {
                this.segments[index / SimpleSegmentedList<T>.SegmentSize][index % SimpleSegmentedList<T>.SegmentSize] = value;
            }
        }

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public object SyncRoot => this;

        public bool IsSynchronized => false;

        object IList.this[int index]
        {
            get
            {
                return this.segments[index / SimpleSegmentedList<T>.SegmentSize][index % SimpleSegmentedList<T>.SegmentSize];
            }

            set
            {
                this.segments[index / SimpleSegmentedList<T>.SegmentSize][index % SimpleSegmentedList<T>.SegmentSize] = (T)value;
            }
        }

        /// <summary>
        /// Adds new element at the end of the list.
        /// </summary>
        /// <param name="item">New element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            int seg = this.count / SimpleSegmentedList<T>.SegmentSize;

            if (seg == this.segmentCount)
            {
                this.AddSegment();
            }

            this.segments[seg][this.count % SimpleSegmentedList<T>.SegmentSize] = item;
            this.count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AddSegment()
        {
            T[] segment = new T[SimpleSegmentedList<T>.SegmentSize];

            if (this.segments == null)
            {
                this.segments = new T[1][];
            }
            else if (this.segments.Length == this.segmentCount)
            {
                Array.Resize(ref this.segments, this.segments.Length * 2);
            }

            this.segments[this.segmentCount++] = segment;
        }

        public int Add(object value)
        {
            this.Add((T)value);

            return this.count - 1;
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
