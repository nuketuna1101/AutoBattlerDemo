using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;

public class PAttackState : IPlayerState
{
    private Player player;
    private Monster targetMonster;      public Monster TargetMonster { get { return targetMonster; } }
    private Coroutine escapeCoroutine;
    private Coroutine attackCoroutine;

    private bool isAttackReady = true;   // �Ϲ� ���� �غ� ����
    private bool isSkillReady = false;  // Ư�� ���� �غ� ����

    public PAttackState(Player player, Monster targetMonster)
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
                player.TransitionState(new PIdleState(player));
                yield break; // �ڷ�ƾ ����
            }

            yield return null;
        }
    }
    private IEnumerator AttackRoutine()
    {
        player.StartCoroutine(SkillCoolDownRoutine());
        while (true)
        {
            yield return null; // ���� �����ӱ��� ���
            if (isSkillReady)
            {
                //player.CastSkill(targetMonster);
                player.SetAnimTrigger("CastSkill");
                DebugOpt.Log("CastSkill! " + Time.time);
                isSkillReady = false;
                player.StartCoroutine(SkillCoolDownRoutine());
            }
            if (isAttackReady)
            {
                //player.BasicAttack(targetMonster);
                player.SetAnimTrigger("BasicAttack");
                DebugOpt.Log("BasicAttack! " + Time.time);
                isAttackReady = false;
                player.StartCoroutine(AttackCoolDownRoutine());
            }
        }
    }
    private IEnumerator AttackCoolDownRoutine()
    {
        yield return new WaitForSeconds(player.attackCooltime);
        isAttackReady = true;
    }
    private IEnumerator SkillCoolDownRoutine()
    {
        yield return new WaitForSeconds(player.skillCooltime);
        isSkillReady = true;
    }
    
}
