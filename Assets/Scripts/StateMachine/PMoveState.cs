using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PMoveState : IPlayerState
{
    private Player player;
    private Monster targetMonster; public Monster TargetMonster { get { return targetMonster; } }
    private Coroutine moveCoroutine;
    public PMoveState(Player player, Monster targetMonster)
    {
        this.player = player;
        this.targetMonster = targetMonster;
    }
    public void Enter()
    {
        player.SetAnimBool("MoveState", true);
        if (moveCoroutine != null)
        {
            player.StopCoroutine(moveCoroutine);
        }
        moveCoroutine = player.StartCoroutine(MoveRoutine());
    }
    public void Exit()
    {
        player.SetAnimBool("MoveState", false);
        if (moveCoroutine != null)
        {
            player.StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            Vector2 direction = (targetMonster.transform.position - player.transform.position).normalized;
            // flip logic �ʿ�
            player.transform.position = Vector2.MoveTowards(player.transform.position, targetMonster.transform.position, player.trackSpeed * Time.deltaTime);
            float distance = Vector2.Distance(player.transform.position, targetMonster.transform.position);
            if (distance <= player.attackRange)
            {
                player.TransitionState(new PAttackState(player, targetMonster));
                yield break;
            }
            else if (distance > player.sightRange)
            {
                player.TransitionState(new PIdleState(player));
                yield break;
            }
            yield return null;
        }
    }
}