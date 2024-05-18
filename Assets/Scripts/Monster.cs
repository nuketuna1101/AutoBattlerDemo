using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    //
    private int health;
    private int attackDamage;
    private float attackNumberPerSecond;
    private float attackRange = 1.0f;
    //
    private float respawnCycle = 5.0f;

    // 추적 관련
    private bool isOnTrack = false;
    private float sightRange = 5;
    private float trackSpeed = 1.0f;



    //
    public Animator anim;
    public SpriteRenderer spriter;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        //rigid = this.GetComponent<Rigidbody2D>();
        spriter = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //
        BattleManager.Instance.RegisterMonster(this);
        //
        //TransitionState(new IdleState(this));
    }

    private void Update()
    {
        //Debug.Log(" CURRENT STATE : " + myState);
    }
    public void TransitionState(IState nextState)
    {
        //if (myState != null)            myState.Exit();
        //myState = nextState;
        //myState.Enter();
    }
    public void SetAnimParam(string paramName, bool boolVal)
    {
        anim.SetBool(paramName, boolVal);
    }
    public void SetAnimParam(string paramName)
    {
        anim.SetTrigger(paramName);
    }
}
