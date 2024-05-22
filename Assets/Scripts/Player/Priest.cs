using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Player
{
    public override PlayerClass playerClass => PlayerClass.Priest;

    protected override void CastSkill()
    {
        //	단일 원거리 치유 스킬(스킬사거리 범위 내 단일 아군 체력을 공격력 250%만큼 회복) 
        BattleManager.Instance.HealAnyPlayerInRange(this, skillRange, attackDamage * 2.5f);
    }
}
