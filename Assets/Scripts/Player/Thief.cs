using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Player
{
    protected override void CastSkill()
    {
        // -	���� ���� ��ų(�ֺ� skillRange ���� ��� ���鿡�� ���ݷ� 100%��ŭ �������� ������.)
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage);

    }
}
