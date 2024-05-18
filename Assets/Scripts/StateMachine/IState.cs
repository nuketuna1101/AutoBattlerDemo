using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


/*
 
     public enum State 
    {
        IDLE,
        MOVE,
        ATTACK,
        DEAD,
        HURT
    }
 
 */
public interface IState
{
    public void Enter();
    public void Exit();
}
