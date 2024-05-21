using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Player : MonoBehaviour
{
    // 모든 플레이어블 캐릭터에 대한 부모 클래스
    [SerializeField]
    private PlayerStatsSO playerStatsSO;
    private float respawnCycle;
    private int health;
    protected int attackDamage;
    public float attackRange;
    public float attackCooltime;
    private float skillRange;
    public float skillCooltime;

    // 추적 관련
    public float sightRange = 5.0f;
    public float trackSpeed = 1.0f;
    public Animator anim;
    public SpriteRenderer spriter;

    public IPlayerState myState;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        InitStatFromSO();
        BattleManager.Instance.RegisterPlayer(this);
        TransitionState(new PIdleState(this));
    }
    protected void InitStatFromSO()
    {
        // SO로부터 스탯 정보 초기화
        respawnCycle = playerStatsSO.respawnCycle;
        health = playerStatsSO.health;
        attackDamage = playerStatsSO.attackDamage;
        attackRange = playerStatsSO.attackRange;
        attackCooltime = playerStatsSO.attackCooltime;
        skillRange = playerStatsSO.skillRange;
        skillCooltime = playerStatsSO.skillCooltime;
    }
    public void TransitionState(IPlayerState nextState)
    {
        if(myState != null) myState.Exit();
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
    public void ResetAnimTrigger(string paramName)
    {
        anim.ResetTrigger(paramName);
    }
    protected void BasicAttack()
    {
        Monster targetMonster = myState.TargetMonster;
        if (targetMonster == null) return;
        BattleManager.Instance.AttackFromPlayerToMonster(this, targetMonster, attackDamage);
    }
    protected abstract void CastSkill();
    public virtual void BeAttacked(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
        SetAnimTrigger("BeAttacked");
    }
    protected virtual void Die()
    {
        SetAnimTrigger("Death");
    }
}