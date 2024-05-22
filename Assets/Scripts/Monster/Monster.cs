using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private MonsterStatsSO monsterStatsSO;
    protected float respawnCycle;
    protected float health;
    protected float attackDamage;
    public float attackCooltime;
    public float attackRange;
    protected float maxHealth;

    [Header("Tracking")]
    public float sightRange = 5.0f;
    public float trackSpeed = 1.0f;

    [Header("Main Components")]
    public Animator anim;
    public SpriteRenderer spriter;
    public IMonsterState myState;

    [Header("Floating HP bar UI")]
    private Slider HPbar;
    private Coroutine updateHPbarCoroutine;
    private const float HPbarHeight = 1.0f;

    protected virtual void Awake()
    {
        anim = this.GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        // 풀링을 통해 이용하기 때문에 활성화 부분 코드는 초기화 부분.
        // 초기화 항목: 스탯 능력치, 배틀매니저 등록, 기본idle 상태 진입, HP Bar UI 할당
        InitStatFromSO();
        InitHPUI();
        BattleManager.Instance.RegisterMonster(this);
        TransitionState(new MIdleState(this));
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
    protected void InitHPUI()
    {
        // 체력바 풀로부터 체력바 ui를 가져와서 캔버스에 붙인 후, 몬스터 오브젝트에 따라다니도록 코루틴 시작.
        Canvas canvas = FindObjectOfType<Canvas>();
        HPbar = HPbarPoolManager.GetFromPool().GetComponent<Slider>();
        HPbar.transform.SetParent(canvas.transform);
        HPbar.maxValue = maxHealth;
        HPbar.value = health;
        CoroutineHelper.RestartCor(this, ref updateHPbarCoroutine, UpdateHPbarRoutine());
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
        // 체력바 HP UI가 오브젝트 따라다니는 코루틴. 그리고 현재 체력에 따라 슬라이더 업데이트
        while (true)
        {
            yield return null;
            Vector3 HPbarPosition = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + HPbarHeight, 0));
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




    public void GetStunned(float duration)
    {
        this.TransitionState(new MStunnedState(this, myState.TargetPlayer, myState, duration));
    }

}
