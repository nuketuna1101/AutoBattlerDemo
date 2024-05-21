using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster
{
    protected override void Start()
    {
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        InitStatFromSO();
        BattleManager.Instance.RegisterMonster(this);
        TransitionState(new MIdleState(this));
    }
    public override void BasicAttack()
    {
        Player targetPlayer = myState.TargetPlayer;
        if (targetPlayer == null) return;

        BattleManager.Instance.AttackFromMonsterToPlayer(this, targetPlayer, attackDamage);
    }
}
