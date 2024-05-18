using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStatsSO", order = 1)]
public class PlayerStatsSO : ScriptableObject
{
    public int health;
    public int attackDamage;
    public float attackRange;
    public float attackCooltime;
    public float skillRange;
    public float skillCooltime;
    public float respawnCycle;
}