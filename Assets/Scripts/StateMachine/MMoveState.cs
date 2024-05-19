using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MMoveState : IMonsterState
{
    private Monster monster;
    private Player targetPlayer; public Player TargetPlayer { get { return targetPlayer; } }
    private Coroutine moveCoroutine;
    public MMoveState(Monster monster, Player targetPlayer)
    {
        DebugOpt.Log("NULL CHECK : " + (monster == null));

        this.monster = monster;
        this.targetPlayer = targetPlayer;
    }
    public void Enter()
    {
        monster.SetAnimBool("MoveState", true);
        if (moveCoroutine != null)
        {
            monster.StopCoroutine(moveCoroutine);
        }
        moveCoroutine = monster.StartCoroutine(MoveRoutine());
    }
    public void Exit()
    {
        monster.SetAnimBool("MoveState", false);
        if (moveCoroutine != null)
        {
            monster.StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            DebugOpt.Log("NULL CHECK : " + (targetPlayer == null));
            Vector2 direction = (targetPlayer.transform.position - monster.transform.position).normalized;
            // flip logic ÇÊ¿ä
            monster.transform.position = Vector2.MoveTowards(monster.transform.position, targetPlayer.transform.position, monster.trackSpeed * Time.deltaTime);
            float distance = Vector2.Distance(monster.transform.position, targetPlayer.transform.position);
            if (distance <= monster.attackRange)
            {
                monster.TransitionState(new MAttackState(monster, targetPlayer));
                yield break;
            }
            else if (distance > monster.sightRange)
            {
                monster.TransitionState(new MIdleState(monster));
                yield break;
            }
            yield return null;
        }
    }
}
