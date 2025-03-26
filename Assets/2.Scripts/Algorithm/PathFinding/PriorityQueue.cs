using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement element, TPriority priority)> _heap = new();

    public int Count => _heap.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        _heap.Add((element, priority));
        int current = _heap.Count - 1;

        while (current > 0)
        {
            int parent = (current - 1) / 2;
            if (_heap[current].priority.CompareTo(_heap[parent].priority) >= 0)
                break;

            Swap(current, parent);
            current = parent;
        }
    }

    public bool TryDequeue(out TElement element, out TPriority priority)
    {
        if (_heap.Count == 0)
        {
            element = default;
            priority = default;
            return false;
        }

        element = _heap[0].element;
        priority = _heap[0].priority;

        int last = _heap.Count - 1;
        _heap[0] = _heap[last];
        _heap.RemoveAt(last);

        int current = 0;
        while (true)
        {
            int left = current * 2 + 1;
            int right = current * 2 + 2;
            int smallest = current;

            if (left < _heap.Count && _heap[left].priority.CompareTo(_heap[smallest].priority) < 0)
                smallest = left;
            if (right < _heap.Count && _heap[right].priority.CompareTo(_heap[smallest].priority) < 0)
                smallest = right;

            if (smallest == current)
                break;

            Swap(current, smallest);
            current = smallest;
        }

        return true;
    }

    public TElement Peek()
    {
        if (_heap.Count == 0)
            throw new InvalidOperationException("PriorityQueue is empty.");
        return _heap[0].element;
    }

    private void Swap(int a, int b)
    {
        (_heap[a], _heap[b]) = (_heap[b], _heap[a]);
    }
}
