using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;

public class MAttackState : IMonsterState
{
    private Monster monster;
    private Player targetPlayer;    public Player TargetPlayer { get { return targetPlayer; } }

    private Coroutine escapeCoroutine;
    private Coroutine attackCoroutine;

    private bool isAttackReady = true;   // 일반 공격 준비 여부

    public MAttackState(Monster monster, Player targetPlayer)
    {
        DebugOpt.Log("NULL CHECK : " + (monster == null));

        this.monster = monster;
        this.targetPlayer = targetPlayer;
    }
    public void Enter()
    {
        if (escapeCoroutine != null)
        {
            monster.StopCoroutine(escapeCoroutine);
        }
        escapeCoroutine = monster.StartCoroutine(EscapeRoutine());

        if (attackCoroutine != null)
        {
            monster.StopCoroutine(attackCoroutine);
        }
        attackCoroutine = monster.StartCoroutine(AttackRoutine());
    }
    public void Exit()
    {
        if (escapeCoroutine != null)
        {
            monster.StopCoroutine(escapeCoroutine);
            escapeCoroutine = null;
        }
        if (attackCoroutine != null)
        {
            monster.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    private IEnumerator EscapeRoutine()
    {
        while (true)
        {
            if (targetPlayer == null)
            {
                monster.TransitionState(new MIdleState(monster));
                yield break; // 코루틴 종료
            }

            yield return null;
        }
    }


    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return null; // 다음 프레임까지 대기
            if (isAttackReady)
            {
                monster.SetAnimTrigger("BasicAttack");
                DebugOpt.Log("BasicAttack! " + Time.time);
                isAttackReady = false;
                monster.StartCoroutine(AttackCoolDownRoutine());
            }
        }
    }
    private IEnumerator AttackCoolDownRoutine()
    {
        yield return new WaitForSeconds(monster.attackCooltime);
        isAttackReady = true;
    }
}
