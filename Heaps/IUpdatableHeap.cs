using System;

namespace lab4.Queues
{
    
    /**
     * Heap with an ability to dinamically replace oldValue
     * with newValue.
     *
     * After value is replaced, the main property of a heap
     * must be recovered.
     *
     * It is used inside IUpdatableQueue for storing data.
     */
    public interface IUpdatableHeap<TElement> : IHeap<TElement>
    where TElement : IComparable<TElement>
    {
        void Update(TElement oldValue, TElement newValue);
    }
}