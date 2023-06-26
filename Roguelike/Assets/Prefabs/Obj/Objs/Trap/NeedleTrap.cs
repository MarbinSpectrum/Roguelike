using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleTrap : Mgr
{
    [SerializeField] private Animator animator;
    [SerializeField] private int damage;
    private int trapStep = 0;
    private bool isDamage = false;
    private Vector2Int pos;
     
    ////////////////////////////////////////////////////////////////////////////////
    /// : 트랩을 초기 상태로맞춘다.
    ////////////////////////////////////////////////////////////////////////////////
    public void Init(Vector2Int pPos)
    {
        trapStep = 0;
        animator.SetTrigger("Stop");
        pos = pPos;
        isDamage = false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 트랩작동 
    ///  조금씩작동함 멈춤->준비->작동 순으로 천천히 작동함
    ////////////////////////////////////////////////////////////////////////////////
    public void Run()
    {
        trapStep++;
        if(trapStep == 5)
        {
            animator.SetTrigger("Ready");
        }
        else if (trapStep == 6)
        {
            animator.SetTrigger("Run");
            isDamage = true;
        }
        else if (trapStep == 9)
        {
            animator.SetTrigger("Stop");
            isDamage = false;
            trapStep = 0;
        }

        if(isDamage)
        {
            if (characterMgr.CharactorGamePos() == pos)
            {
                characterMgr.Hit(damage);
                characterMgr.PlayHitAni();
            }
        }
    }
}
