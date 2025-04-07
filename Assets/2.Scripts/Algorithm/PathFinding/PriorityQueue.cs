using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private readonly List<(TElement element, TPriority priority)> _heap = new List<(TElement, TPriority)>();
    public int Count => _heap.Count;
    
    public bool Enqueue(TElement element, TPriority priority)
    {
        _heap.Add((element, priority));
        
        var index = _heap.Count - 1;

        while (index > 0)
        {
            var parent = (index - 1) / 2;
            
            if (_heap[index].priority.CompareTo(_heap[parent].priority) >= 0)
                return true;

            (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
            index = parent;
        }

        return false;
    }

    public bool TryDequeue(out TElement element, out TPriority priority)
    {
        if (_heap.Count <= 0)
        {
            element = default;
            priority = default;
            return false;
        }

        element = _heap[0].Item1;
        priority = _heap[0].Item2;
        
        var lastElement = _heap[^1];
        _heap[0] = lastElement;
        _heap.RemoveAt(_heap.Count - 1);

        var index = 0;
        var count = _heap.Count;

        while (true)
        {
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var current = index;
            
            if (left < count && _heap[left].priority.CompareTo(_heap[current].priority) < 0)
            {
                current = left;
            }
            if (right < count && _heap[right].priority.CompareTo(_heap[current].priority) < 0)
            {
                current = right;
            }
            if (current == index)
            {
                return true;
            }

            (_heap[index], _heap[current]) = (_heap[current], _heap[index]);
            index = current;
        }
    }
}
