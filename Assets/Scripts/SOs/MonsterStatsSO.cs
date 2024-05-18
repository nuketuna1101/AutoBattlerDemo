using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStatsSO", menuName = "ScriptableObjects/MonsterStatsSO")]
public class MonsterStatsSO : ScriptableObject
{
    public float respawnCycle;
    public int health;
    public int attackDamage;
    public float attackRange;
    public float attackCooltime;
}
