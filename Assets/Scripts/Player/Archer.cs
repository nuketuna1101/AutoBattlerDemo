using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{
    public override PlayerClass playerClass => PlayerClass.Archer;
    protected override void CastSkill()
    {
        //	단일 원거리 공격 스킬(스킬사거리 범위 내 단일 대상에게 공격력 250%만큼 데미지) 
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage * 2.5f);
    }
}
