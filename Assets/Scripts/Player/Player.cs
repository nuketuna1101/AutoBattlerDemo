using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Player : MonoBehaviour
{
    // ��� �÷��̾�� ĳ���Ϳ� ���� �θ� Ŭ����
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

    // ���� ����
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
        // SO�κ��� ���� ���� �ʱ�ȭ
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
            // �̹� ���� �÷��̾ ���� ���͵��� ���� ����. �ٷ� ��Ʋ���� ���ܽ����ֱ�.
            BattleManager.Instance.DeregisterPlayer(this);
        }
    }
    protected virtual void Die()
    {
        SetAnimTrigger("Death");
    }

    public void ReturnToPool()
    {
        // ��Ȱ Ÿ�̸� ������ ������Ʈ�� Ǯ�� ȸ��
        BattleManager.Instance.RespawnPlayer(this);
        PlayerPoolManager.ReturnToPool(this.gameObject);
    }
}