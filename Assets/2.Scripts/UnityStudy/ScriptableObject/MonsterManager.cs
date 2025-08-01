using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private List<Monster> monsters;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            monsters.ForEach(x => x.PrintData());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            monsters[Random.Range(0, monsters.Count)].LevelUp();
        }
    }
}
