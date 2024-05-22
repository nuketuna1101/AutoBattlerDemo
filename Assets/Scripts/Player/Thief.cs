using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Player
{
    public override PlayerClass playerClass => PlayerClass.Thief;

    protected override void CastSkill()
    {
        // ���� ���� ��ų(�ֺ� skillRange ���� ��� ���鿡�� ���ݷ� 100%��ŭ �������� ������.)
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackAreaFromPlayer(this, skillRange, attackDamage);
    }
}
