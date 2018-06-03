using System;

namespace lab4.Queues
{
    public interface IHeap<TElement> 
    where TElement : IComparable<TElement>
    {
        int Count { get; }
        void Push(TElement element);
        TElement Pop();
        TElement Peek();
        bool Contains(TElement element);
        bool IsEmpty();
    }
}