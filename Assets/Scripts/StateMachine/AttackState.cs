using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;

public class AttackState : IState
{
    private Player player;
    private Monster targetMonster;
    private Coroutine escapeCoroutine;
    private Coroutine attackCoroutine;

    private bool isAttackReady = true;   // 일반 공격 준비 여부
    private bool isSkillReady = false;  // 특수 공격 준비 여부

    public AttackState(Player player, Monster targetMonster)
    {
        this.player = player;
        this.targetMonster = targetMonster;
    }
    public void Enter()
    {
        if (escapeCoroutine != null)
        {
            player.StopCoroutine(escapeCoroutine);
        }
        escapeCoroutine = player.StartCoroutine(EscapeRoutine());

        if (attackCoroutine != null)
        {
            player.StopCoroutine(attackCoroutine);
        }
        attackCoroutine = player.StartCoroutine(AttackRoutine());
    }
    public void Exit()
    {
        if (escapeCoroutine != null)
        {
            player.StopCoroutine(escapeCoroutine);
            escapeCoroutine = null;
        }
        if (attackCoroutine != null)
        {
            player.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    private IEnumerator EscapeRoutine()
    {
        while (true)
        {
            if (targetMonster == null)
            {
                player.TransitionState(new IdleState(player));
                yield break; // 코루틴 종료
            }

            yield return null;
        }
    }
    private IEnumerator AttackRoutine()
    {
        player.StartCoroutine(skillCoolDownRoutine());
        while (true)
        {
            yield return null; // 다음 프레임까지 대기
            if (isSkillReady)
            {
                player.CastSkill(targetMonster);
                player.SetAnimParam("CastSkill");
                DebugOpt.Log("CastSkill! " + Time.time);
                isSkillReady = false;
                player.StartCoroutine(skillCoolDownRoutine());
            }
            else if (isAttackReady && !isSkillReady)
            {
                player.BasicAttack(targetMonster);
                player.SetAnimParam("BasicAttack");
                DebugOpt.Log("BasicAttack! " + Time.time);
                isAttackReady = false;
                player.StartCoroutine(attackCoolDownRoutine());
            }
        }
    }
    private IEnumerator attackCoolDownRoutine()
    {
        yield return new WaitForSeconds(player.attackCooltime);
        isAttackReady = true;
    }
    private IEnumerator skillCoolDownRoutine()
    {
        yield return new WaitForSeconds(player.skillCooltime);
        isSkillReady = true;
    }
}
