using System;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData data;
    private MonsterData _cloneData;

    private void Start()
    {
        _cloneData = Instantiate(data);
    }

    public void PrintData()
    {
        Debug.Log($"{_cloneData.name} 정보 ===========");
        Debug.Log(_cloneData.MonsterName);
        Debug.Log(_cloneData.Hp);
        Debug.Log(_cloneData.Damage);
        Debug.Log(_cloneData.LV);
        Debug.Log("==========================");
    }

    public void LevelUp()
    {
        ++_cloneData.LV;
    }
}
