using System;

namespace lab4.Queues
{
    public class UpdatablePriorityQueue<TElement> : IUpdatableQueue<TElement>
    where TElement : IComparable<TElement>
    {
        private UpdatableMaxHeap<TElement> _maxHeap;
        private int _size;
        
        public int Count => _size;

        public UpdatablePriorityQueue()
        {
            _maxHeap = new UpdatableMaxHeap<TElement>();
            _size = 0;
        }

        public UpdatablePriorityQueue(TElement[] elements)
        {
            _maxHeap = new UpdatableMaxHeap<TElement>(elements);
            _size = elements.Length;
        }
        public TElement Dequeue()
        {
            if (_size == 0)
                return default(TElement);
            --_size;
            return _maxHeap.Pop();
        }

        public TElement Peek()
        {
            return _maxHeap.Peek();
        }

        public void Enqueue(TElement element)
        {
            _size++;
            _maxHeap.Push(element);
        }

        public bool Contains(TElement element)
        {
            return _maxHeap.Contains(element);
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }
        
        
        public void Update(TElement oldValue, TElement newValue)
        {
            if (!Contains(oldValue))
                return;
            
            _maxHeap.Update(oldValue, newValue);
        }
    }
}