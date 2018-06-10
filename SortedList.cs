using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;

namespace lab4
{
    public class SortedList<TElement>
    {
        private IComparer<TElement> _comparer;
        private List<TElement> _array;
        public int Count => _array.Count;
        
        public SortedList(IComparer<TElement> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("Comparer cannot be null");
            _array = new List<TElement>();
        }

        public void Add(TElement item)
        {
            if (_array.Count == 0)
            {
                _array.Add(item);
                return;
            }

            if (_comparer.Compare(item, _array[_array.Count - 1]) > 0)
            {
                _array.Add(item);
                return;
            }

            if (_comparer.Compare(item, _array[0]) < 0)
            {
                AddAfter(-1, item);
                return;
            }

            for (int i = 1; i < _array.Count; i++)
            {
                var greaterOrEqualsPrev = _comparer.Compare(item, _array[i - 1]) >= 0;
                var lessOrEqualsCurr = _comparer.Compare(item, _array[i]) <= 0;

                if (greaterOrEqualsPrev && lessOrEqualsCurr)
                {
                    AddAfter(i, item);
                    return;
                }
            }
        }
    
        /**
         * Shifts all items after leftBound one
         * position left and sets item to the position
         * lefBound + 1
         * 
         */
        private void AddAfter(int leftBound, TElement item)
        {
            if (leftBound >= _array.Count || leftBound < -1)
                return;
            
            int rightBound = leftBound + 1;
            
            if (rightBound >= _array.Count)
            {
                _array.Add(item);
                return;
            }
            
            _array.Add(_array[_array.Count - 1]);
            var t1 = _array[rightBound];
            var t2 = _array[rightBound];
            for (int i = rightBound; i < _array.Count - 1; i++)
            {
                t1 = _array[i];
                _array[i] = t2;
                t2 = t1;
            }

            _array[rightBound] = item;        
        }
        
        public bool Contains(TElement item) => _array.Contains(item);

        public TElement this[int index] => _array[index];

        public TElement RemoveAt(int index)
        {
            var t = _array[index];
            _array.RemoveAt(index);
            return t;
        }


    }
}