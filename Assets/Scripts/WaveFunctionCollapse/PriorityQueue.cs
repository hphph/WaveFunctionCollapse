using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    List<Tuple<T, int>> elements;

    public int Count => elements.Count;

    public PriorityQueue()
    {
        elements = new List<Tuple<T, int>>();
    }

    public void Add(T item, int key)
    {
        Tuple<T, int> element = new Tuple<T, int>(item, key);
        int sameItemIndex = elements.FindIndex( e => ReferenceEquals(e.Item1, item));
        if(sameItemIndex != -1)
        {
            if(key != elements[sameItemIndex].Item2) elements.RemoveAt(sameItemIndex);
            else return;
        }
        elements.Add(element);
    }

    public T ExtractMin()
    {
        if(elements.Count == 0) return default(T);
        int lowestValue = elements[0].Item2;
        int lowestValueIndex = 0;

        for(int i = 0; i < elements.Count; i++)
        {
            if(elements[i].Item2 < lowestValue)
            {
                lowestValue = elements[i].Item2;
                lowestValueIndex = i;
            }
        }
        T minElement = elements[lowestValueIndex].Item1;
        elements.RemoveAt(lowestValueIndex);

        return minElement;
    }
}
