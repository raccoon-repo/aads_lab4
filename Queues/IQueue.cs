using System;

namespace lab4.Queues
{
    public interface IQueue<TElement>
    {
        int Count { get; }
        
        TElement Dequeue();
        TElement Peek();
        void Enqueue(TElement element);
        bool Contains(TElement element);
        bool IsEmpty();
    }
}