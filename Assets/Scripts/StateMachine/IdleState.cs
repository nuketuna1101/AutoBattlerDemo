using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class IdleState : IState
{
    private Player player;
    private Coroutine idleCoroutine;

    public IdleState(Player player)
    {
        this.player = player;
    }

    public void Enter() 
    {
        if (idleCoroutine != null)
        {
            player.StopCoroutine(idleCoroutine);
        }
        idleCoroutine = player.StartCoroutine(IdleRoutine());
    }

    public void Exit()
    {
        if (idleCoroutine != null)
        {
            player.StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            Monster monster = BattleManager.Instance.FindNearestMonster(player);
            if (monster != null)
            {
                player.TransitionState(new MoveState(player, monster));
                yield break;
            }
            yield return null;
            //yield return new WaitForSeconds(0.1f);
        }
    }

}
