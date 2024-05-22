using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    public override PlayerClass playerClass => PlayerClass.Knight;

    protected override void CastSkill()
    {
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage);

        /* ���� ȿ�� ������ �߰� ���� �ʿ� */
    }
}
