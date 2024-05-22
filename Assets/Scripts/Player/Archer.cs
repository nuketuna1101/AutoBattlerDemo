using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{
    public override PlayerClass playerClass => PlayerClass.Archer;
    protected override void CastSkill()
    {
        //	���� ���Ÿ� ���� ��ų(��ų��Ÿ� ���� �� ���� ��󿡰� ���ݷ� 250%��ŭ ������) 
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage * 2.5f);
    }
}
