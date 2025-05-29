using System.Collections;
using System.Collections.Generic;

public class CustomEnumerator : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        yield return 1;
        yield return 1;
        yield return 1;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
