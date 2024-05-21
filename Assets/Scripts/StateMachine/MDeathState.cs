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
        CoroutineHelper.RestartCor(monster, ref deathCoroutine, DeathCoroutine());
    }
    public void Exit()
    {
        CoroutineHelper.StopCor(monster, ref deathCoroutine);
    }
    private IEnumerator DeathCoroutine()
    {
        yield return null;
        monster.SetAnimTrigger("Death");
        BattleManager.Instance.DeregisterMonster(monster);
        MonsterPoolManager.ReturnToPool(monster.gameObject);
    }
}
