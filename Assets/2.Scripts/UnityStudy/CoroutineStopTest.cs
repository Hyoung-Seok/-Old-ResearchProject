using System;
using System.Collections;
using UnityEngine;

public class CoroutineStopTest : MonoBehaviour
{
    private IEnumerator _testEnumerator;
    private Coroutine _testCoroutine;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);
    
    private void Start()
    {
        // _testEnumerator = TestRoutine();
        // StartCoroutine(_testEnumerator);
        
        _testCoroutine = StartCoroutine(TestRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopCoroutine(_testCoroutine);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _testCoroutine = StartCoroutine(TestRoutine());
        }
    }

    private IEnumerator TestRoutine()
    {
        var i = 0;
        Debug.Log("코루틴 시작");
        
        while (i < 10)
        {
            Debug.Log(++i);
            yield return _waitForSeconds;
        }
        
        Debug.Log("코루틴 중지");
    }
}
