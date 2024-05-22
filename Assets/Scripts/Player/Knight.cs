using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    public override PlayerClass playerClass => PlayerClass.Knight;
    private const float stunDuration = 1.0f;
    protected override void CastSkill()
    {
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage);
        BattleManager.Instance.GiveStunned(this, targetMonster, stunDuration);
    }
}
