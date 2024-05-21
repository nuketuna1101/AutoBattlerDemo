using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDeathState : IMonsterState
{
    private Monster monster;
    private Player targetPlayer; public Player TargetPlayer { get { return targetPlayer; } }
    private Coroutine deathCoroutine;

    public MDeathState(Monster monster)
    {
        this.monster = monster;
    }
    public void Enter()
    {
        if (deathCoroutine != null)
        {
            monster.StopCoroutine(deathCoroutine);
        }
        deathCoroutine = monster.StartCoroutine(DeathCoroutine());
    }
    public void Exit()
    {
        if (deathCoroutine != null)
        {
            monster.StopCoroutine(deathCoroutine);
            deathCoroutine = null;
        }
    }
    private IEnumerator DeathCoroutine()
    {
        yield return null;
        monster.SetAnimTrigger("Death");
        BattleManager.Instance.DeregisterMonster(monster);
        MonsterPoolManager.ReturnToPool(monster.gameObject);
    }
}
