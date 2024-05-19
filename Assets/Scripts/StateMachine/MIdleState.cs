using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MIdleState : IMonsterState
{
    private Monster monster;
    private Player targetPlayer; public Player TargetPlayer { get { return targetPlayer; } }

    private Coroutine idleCoroutine;

    public MIdleState(Monster monster)
    {
        this.monster = monster;
        DebugOpt.Log("NULL CHECK : " + (monster == null));
    }

    public void Enter()
    {
        if (idleCoroutine != null)
        {
            monster.StopCoroutine(idleCoroutine);
        }
        idleCoroutine = monster.StartCoroutine(IdleRoutine());
    }

    public void Exit()
    {
        if (idleCoroutine != null)
        {
            monster.StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            Player player = BattleManager.Instance.FindNearestPlayer(monster);
            if (player != null)
            {
                monster.TransitionState(new MMoveState(monster, player));
                yield break;
            }
            yield return null;
            //yield return new WaitForSeconds(0.1f);
        }
    }

}
