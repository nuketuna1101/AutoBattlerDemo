using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterStatsSO monsterStatsSO;
    protected float respawnCycle;
    protected int health;
    protected int attackDamage;
    public float attackCooltime;
    public float attackRange;
    //

    // ���� ����
    public float sightRange = 5.0f;
    public float trackSpeed = 1.0f;
    public Animator anim;
    public SpriteRenderer spriter;

    public IMonsterState myState;

    protected virtual void Start()
    {
        anim = this.GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //
        InitStatFromSO();
        //
        BattleManager.Instance.RegisterMonster(this);
        //
        TransitionState(new MIdleState(this));
    }
    protected void InitStatFromSO()
    {
        respawnCycle = monsterStatsSO.respawnCycle;
        health = monsterStatsSO.health;
        attackDamage = monsterStatsSO.attackDamage;
        attackCooltime = monsterStatsSO.attackCooltime;
        attackRange = monsterStatsSO.attackRange;
    }

    public void TransitionState(IMonsterState nextState)
    {
        if (myState != null) myState.Exit();
        myState = nextState;
        myState.Enter();
    }
    public void SetAnimBool(string paramName, bool boolVal)
    {
        anim.SetBool(paramName, boolVal);
    }
    public void SetAnimTrigger(string paramName)
    {
        anim.SetTrigger(paramName);
    }
    public abstract void BasicAttack();
    public abstract void BeAttacked(int damage);
    protected abstract void Die();
    public void ReturnToPool()
    {
        MonsterPoolManager.ReturnToPool(this.gameObject);
    }
}
