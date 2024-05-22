using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Player : MonoBehaviour
{
    // 모든 플레이어블 캐릭터에 대한 부모 클래스
    [SerializeField]
    private PlayerStatsSO playerStatsSO;
    //public readonly PlayerClass playerClass;
    public abstract PlayerClass playerClass { get; }
    public float respawnCycle;
    protected float health;
    protected float attackDamage;
    public float attackRange;
    public float attackCooltime;
    public float skillRange;
    public float skillCooltime;

    // 추적 관련
    public float sightRange = 5.0f;
    public float trackSpeed = 1.0f;
    public Animator anim;
    public SpriteRenderer spriter;

    public IPlayerState myState;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
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
    public virtual void BeAttacked(float damage)
    {
        health -= damage;
        if (health > 0)
        {
            SetAnimTrigger("BeAttacked");
        }
        else
        {
            Die();
            // 이미 죽은 플레이어에 대한 몬스터들의 로직 방지. 바로 배틀에서 제외시켜주기.
            BattleManager.Instance.DeregisterPlayer(this);
        }
    }
    protected virtual void Die()
    {
        SetAnimTrigger("Death");
    }

    public void ReturnToPool()
    {
        // 부활 타이머 돌리고 오브젝트는 풀로 회수
        BattleManager.Instance.RespawnPlayer(this);
        PlayerPoolManager.ReturnToPool(this.gameObject);
    }
}