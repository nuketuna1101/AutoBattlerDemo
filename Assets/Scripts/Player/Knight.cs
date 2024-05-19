using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    protected override void CastSkill()
    {
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage);

        /* 스턴 효과 관련한 추가 로직 필요 */
    }
}
