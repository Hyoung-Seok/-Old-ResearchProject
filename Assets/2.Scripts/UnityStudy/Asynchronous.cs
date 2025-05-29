using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Asynchronous : MonoBehaviour
{
    private void Start()
    {
        _ = PrintNumberAsync("A");
        _ = PrintNumberAsync("B");

        var customEnumerator = new CustomEnumerator();
        foreach (var temp in customEnumerator)
        {
            
        }
    }

    private async Task PrintNumberAsync(string name)
    {
        for (var i = 0; i <= 10; ++i)
        {
            Debug.Log($"{name} : {i}");
            await Task.Delay(300);
        }
    }
}
