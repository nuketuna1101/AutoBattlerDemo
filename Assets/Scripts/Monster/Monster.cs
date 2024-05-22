using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterStatsSO monsterStatsSO;
    protected float respawnCycle;
    protected float health;
    protected float attackDamage;
    public float attackCooltime;
    public float attackRange;
    //
    protected float maxHealth;


    // 추적 관련
    public float sightRange = 5.0f;
    public float trackSpeed = 1.0f;
    public Animator anim;
    public SpriteRenderer spriter;
    public IMonsterState myState;

    public GameObject HPbarPrefab;
    private Slider HPbar;
    private Coroutine updateHPbarCoroutine;

    protected virtual void Awake()
    {
        anim = this.GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        InitStatFromSO();
        BattleManager.Instance.RegisterMonster(this);
        TransitionState(new MIdleState(this));

        Canvas canvas = FindObjectOfType<Canvas>();
        HPbar = HPbarPoolManager.GetFromPool().GetComponent<Slider>();
        HPbar.transform.SetParent(canvas.transform);
        HPbar.maxValue = maxHealth;
        HPbar.value = health;
        CoroutineHelper.RestartCor(this, ref updateHPbarCoroutine, UpdateHPbarRoutine());
    }
    protected void InitStatFromSO()
    {
        respawnCycle = monsterStatsSO.respawnCycle;
        health = monsterStatsSO.health;
        attackDamage = monsterStatsSO.attackDamage;
        attackCooltime = monsterStatsSO.attackCooltime;
        attackRange = monsterStatsSO.attackRange;
        maxHealth = monsterStatsSO.health;
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
    public void ResetAnimTrigger(string paramName)
    {
        anim.ResetTrigger(paramName);
    }
    public abstract void BasicAttack();
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
            BattleManager.Instance.DeregisterMonster(this);
        }
    }
    protected virtual void Die()
    {
        SetAnimTrigger("Death");
        //this.TransitionState(new MDeathState(this));
    }
    public void ReturnToPool()
    {
        // 부활 타이머 돌리고 오브젝트는 풀로 회수
        BattleManager.Instance.DeregisterMonster(this);
        MonsterPoolManager.ReturnToPool(this.gameObject);
    }

    private IEnumerator UpdateHPbarRoutine()
    {
        while (true)
        {
            yield return null;
            Vector3 HPbarPosition = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1, 0));
            HPbar.transform.position = HPbarPosition;
            if (HPbar != null)
            {
                HPbar.value = health;
            }
        }
    }
    private void OnDisable()
    {
        CoroutineHelper.StopCor(this, ref updateHPbarCoroutine);
        if (HPbar != null)
            HPbarPoolManager.ReturnToPool(HPbar.gameObject);
    }
}
