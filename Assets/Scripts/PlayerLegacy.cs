using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerLegacy : MonoBehaviour
{
    // 모든 플레이어블 캐릭터에 대한 부모 클래스
    private float respawnCycle = 5.0f;
    private int health = 400;
    private int attackDamage = 5;
    public float attackRange = 1.0f;
    public float attackCooltime = 1.5f;
    private float skillRange = 1.0f;
    public float skillCooltime = 3.0f;

    // 추적 관련
    public float sightRange = 5.0f;
    public float trackSpeed = 1.0f;
    public Animator anim;
    public SpriteRenderer spriter;

    public IState myState;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //
        //BattleManager.Instance.RegisterPlayer(this);
        //
        //TransitionState(new IdleState(this));
    }

    private void Update()
    {
        DebugOpt.Log(" CURRENT STATE : " + myState);
    }
    public void TransitionState(IState nextState)
    {
        if (myState != null)
            myState.Exit();
        myState = nextState;
        myState.Enter();
    }
    public void SetAnimParam(string paramName, bool boolVal)
    {
        anim.SetBool(paramName, boolVal);
    }
    public void SetAnimParam(string paramName)
    {
        anim.SetTrigger(paramName);
    }


    public void BasicAttack(Monster monster)
    {

    }
    public void CastSkill(Monster monster)
    {

    }

}