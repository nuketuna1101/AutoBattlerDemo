using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Player
{
    public override PlayerClass playerClass => PlayerClass.Thief;

    protected override void CastSkill()
    {
        // 범위 공격 스킬(주변 skillRange 내의 모든 적들에게 공격력 100%만큼 데미지를 입힌다.)
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackAreaFromPlayer(this, skillRange, attackDamage);
    }
}
