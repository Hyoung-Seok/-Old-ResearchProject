using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private string monsterName;
    [SerializeField] private float hp;
    [SerializeField] private float damage;
    public int LV;

    public string MonsterName => monsterName;
    public float Hp => hp;
    public float Damage => damage;
}
