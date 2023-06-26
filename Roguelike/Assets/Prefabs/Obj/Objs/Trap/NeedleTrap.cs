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
    /// : Ʈ���� �ʱ� ���·θ����.
    ////////////////////////////////////////////////////////////////////////////////
    public void Init(Vector2Int pPos)
    {
        trapStep = 0;
        animator.SetTrigger("Stop");
        pos = pPos;
        isDamage = false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Ʈ���۵� 
    ///  ���ݾ��۵��� ����->�غ�->�۵� ������ õõ�� �۵���
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
