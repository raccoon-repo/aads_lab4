using System;

namespace lab4.Queues
{
    /**
     * Represents a queue where priorities can be
     * updated at a runtime.
     *
     * After updating priorities,
     * priority queue must be valid
     */
    
    public interface IUpdatableQueue<TElement> : IQueue <TElement> 
    {
        void Update(TElement oldValue, TElement newValue);
    }
}