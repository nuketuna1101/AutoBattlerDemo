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
        DebugOpt.Log(this + " BasicAttack called!");
        Player targetPlayer = myState.TargetPlayer;
        if (targetPlayer == null) return;

        BattleManager.Instance.AttackFromMonsterToPlayer(this, targetPlayer, attackDamage);
    }
    public override void BeAttacked(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
        SetAnimTrigger("BeAttacked");
    }
    protected override void Die()
    {
        SetAnimTrigger("Death");
    }
}
