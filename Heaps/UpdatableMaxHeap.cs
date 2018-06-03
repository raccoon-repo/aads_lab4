using System;
using System.Collections.Generic;

namespace lab4.Queues
{
    public class UpdatableMaxHeap<TElement> : IUpdatableHeap<TElement>
    where TElement : IComparable<TElement>
    {
        private List<TElement> _elements;
        private int _size;

        public int Count => _size;

        public UpdatableMaxHeap()
        {
            _elements = new List<TElement>();
            _size = 0;
        }

        public UpdatableMaxHeap(TElement[] elements)
        {
            _elements = new List<TElement>(elements);
            _size = elements.Length;
            
            for (int i = _size / 2; i >= 0; i--)
                SiftDown(i);
        }

        public void Push(TElement element)
        {
            _elements.Add(element);
            _size++;
            SiftUp(_elements.Count - 1);
        }

        public TElement Pop()
        {
            if (_size == 0)
                return default(TElement);
            
            var res = _elements[0];
            _elements[0] = _elements[_elements.Count - 1];
            _elements.RemoveAt(_elements.Count - 1);
            _size--;
            SiftDown(0);


            return res;
        }

        public TElement Peek()
        {
            return _size == 0 ? default(TElement) : _elements[0];
        }

        public bool Contains(TElement element)
        {
            foreach (var t in _elements)
                if (t.Equals(element))
                    return true;

            return false;
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }

        private void SiftDown(int i)
        {
            int leftChild;
            int rightChild;
            int parent;

            while (true)
            {
                leftChild = i * 2 + 1;
                rightChild = i * 2 + 2;
                parent = i;

                if (leftChild < _size && _elements[leftChild].CompareTo(_elements[parent]) > 0)
                    parent = leftChild;

                if (rightChild < _size && _elements[rightChild].CompareTo(_elements[parent]) > 0)
                    parent = rightChild;

                if (parent == i)
                    break;


                var t = _elements[i];
                _elements[i] = _elements[parent];
                _elements[parent] = t;
                i = parent;
            }
        }

        private void SiftUp(int i)
        {
            if (i == 0)
                return;

            int parent = i % 2 == 1 ? i / 2 : i / 2 - 1;

            while (_elements[parent].CompareTo(_elements[i]) < 0)
            {
                var t = _elements[parent];
                _elements[parent] = _elements[i];
                _elements[i] = t;

                i = parent;
                
                if (i == 0)
                    break;
                
                parent = i % 2 == 1 ? i / 2 : i / 2 - 1;
               
            }
        }
        
        public void Update(TElement oldValue, TElement newValue)
        {
            if (!Contains(oldValue))
                return;

            int i;
            for (i = 0; i < _size; i++)
                if (_elements[i].Equals(oldValue))
                    break;

            oldValue = _elements[i];
            _elements[i] = newValue;
            
            if (oldValue.CompareTo(newValue) > 0)
            {
                SiftDown(i);
                return;
            }

            if (oldValue.CompareTo(newValue) < 0)
            {
                SiftUp(i);
                return;
            }
        }
    }
}