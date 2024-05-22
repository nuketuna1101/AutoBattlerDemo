using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Player
{
    public override PlayerClass playerClass => PlayerClass.Priest;

    protected override void CastSkill()
    {
        //	���� ���Ÿ� ġ�� ��ų(��ų��Ÿ� ���� �� ���� �Ʊ� ü���� ���ݷ� 250%��ŭ ȸ��) 
        BattleManager.Instance.HealAnyPlayerInRange(this, skillRange, attackDamage * 2.5f);
    }
}
