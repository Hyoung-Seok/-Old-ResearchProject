using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private readonly List<(TElement, TPriority)> _heap = new List<(TElement, TPriority)>();
    public int Count => _heap.Count;
    
    public bool Enqueue(TElement element, TPriority priority)
    {
        // 가장 마지막 위치에 삽입
        _heap.Add((element, priority));
        
        // 스왑 시작
        var index = _heap.Count - 1;

        while (index > 0)
        {
            var parent = (index - 1) / 2;
            
            // 현재 값이 부모의 값보다 크거나 같다면
            if (_heap[index].Item2.CompareTo(_heap[parent].Item2) >= 0)
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

        // Dequeue한 원소 제거. 원소가 하나만 남아있다면, 자연스럽게 제거됨
        var lastElement = _heap[^1];
        _heap[0] = lastElement;
        _heap.RemoveAt(_heap.Count - 1);

        var index = 0;
        var count = _heap.Count;
        // 정렬 시작
        while (true)
        {
            // 좌측, 우측 자식 인덱스 자식
            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var current = index;
            
            // 왼쪽 자식이 현재 탐색 위치의 우선순위보다 크다면
            if (left < count && _heap[left].Item2.CompareTo(_heap[current].Item2) < 0)
            {
                current = left;
            }
            // 우측 자식이 현재 탐색 위치의 우선순위보다 크다면
            if (right < count && _heap[right].Item2.CompareTo(_heap[current].Item2) < 0)
            {
                current = right;
            }
            // 왼쪽, 오른쪽 자식보다 현재 값이 작다면 
            if (current == index)
            {
                return true;
            }

            (_heap[index], _heap[current]) = (_heap[current], _heap[index]);
            index = current;
        }
    }
}
