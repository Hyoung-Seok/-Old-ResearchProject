using System;
using System.Collections;
using UnityEngine;

public class CoroutineStopTest : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("TestRoutine");
    }

    private IEnumerator TestRoutine()
    {
        var i = 0;
        
        while (true)
        {
            Debug.Log(++i);

            if (i > 1000)
            {
                Destroy(gameObject);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
