using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonsterObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SoundObj hitSE;
    [SerializeField]
    private SoundObj dieSE;

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �ൿ�� ó���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public override void RunMonster(List<Vector2Int> pRoute)
    {
        if (pRoute == null || pRoute.Count == 0)
            return;

        MapManager mapManager = MapManager.instance;

        if (alive == false)
        {
            monsterSpr.enabled = false;
            return;
        }

        if (stun)
        {
            //���� �����̴�.
            //���ϻ��¿��� �����.
            //����鼭 �̵����ð� ���ݽ����� �ʱ�ȭ�ȴ�.
            attackStack = 0;
            moveStack = 0;

            stun = false;
            return;
        }

        if (sleep)
        {
            if (range > pRoute.Count)
            {
                //�÷��̾ �νĹ������� ���Դ�.
                //�ῡ�� �����.
                AwakeMonster();
            }
            return;
        }

        Vector2Int gamePos = pRoute[pRoute.Count - 1];
        if (monsterMgr.IsMonster(gamePos.x, gamePos.y))
        {
            //�ش���ġ�� ���Ͱ� �ִ�. �̵����� �ʴ´�.
            return;
        }
        if (monsterMgr.IsMoveToPos(gamePos.x, gamePos.y))
        {
            //�ش���ġ�� ���Ͱ� �̵��ϱ�� �ߴ�. �̵����� �ʴ´�.
            return;
        }
        if (mapManager.IsWall(gamePos.x, gamePos.y))
        {
            //�ش���ġ�� ���̴� �̵����� �ʴ´�.
            return;
        }
        if(chestMgr.IsChest(gamePos.x,gamePos.y))
        {
            //���ڴ� �μ�������. �̵����� �ʴ´�.
            return;
        }
        if (gamePos == characterMgr.CharactorGamePos())
        {
            //ĳ���Ͱ� ���ݹ����� �ִ�.
            //�̵������ʰ� �÷��̾ �����Ѵ�.
            if (attackStack < attackDelay)
            {
                //���ݽ����� �� ������ �̵� �Ѵ�.
                attackStack++;
                return;
            }

            //ĳ���Ͱ� ���ݹ����� �ִ�.
            //�̵������ʰ� �÷��̾ �����Ѵ�.

            //�÷��̾��� ���⿡ ���� �ִϸ��̼��� �����Ѵ�.
            if (pos.x + 1 == gamePos.x)
            {
                //�����ʿ� �÷��̾ �ִ�.
                animator.SetTrigger("Attack_Side");
                monsterSpr.flipX = false;
            }
            else if (pos.x - 1 == gamePos.x)
            {
                //���ʿ� �÷��̾ �ִ�.
                animator.SetTrigger("Attack_Side");
                monsterSpr.flipX = true;
            }
            else if (pos.y - 1 == gamePos.y)
            {
                //�Ʒ��ʿ� �÷��̾ �ִ�.
                animator.SetTrigger("Attack_Front");
                monsterSpr.flipX = false;
            }
            else if (pos.y + 1 == gamePos.y)
            {
                //���ʿ� �÷��̾ �ִ�.
                animator.SetTrigger("Attack_Back");
                monsterSpr.flipX = false;
            }

            characterMgr.Hit((int)damage);





            //���ݽ� �̵����ð� ���ݽ����� �ʱ�ȭ�ȴ�.
            attackStack = 0;
            moveStack = 0;
            return;
        }
        if (moveStack < moveDelay)
        {
            //�̵������� �� ������ �̵� �Ѵ�.
            moveStack++;
            return;
        }

        if(jarMgr.IsJar(gamePos.x, gamePos.y))
        {
            //�̵���ġ�� �׾Ƹ��� ������ �׾Ƹ��� �μ���.
            Jar jarObj = jarMgr.GetJarObj(gamePos);
            jarObj.RemoveJarObj();
        }

        //�÷��̾��� ���⿡ ���� ��������Ʈ�� �����´�.
        if (pos.x + 1 == gamePos.x)
        {
            //�����ʿ� �÷��̾ �ִ�.
            monsterSpr.flipX = false;
        }
        else if (pos.x - 1 == gamePos.x)
        {
            //���ʿ� �÷��̾ �ִ�.
            monsterSpr.flipX = true;
        }
        else if (pos.y - 1 == gamePos.y)
        {
            //�Ʒ��ʿ� �÷��̾ �ִ�.
            monsterSpr.flipX = false;
        }
        else if (pos.y + 1 == gamePos.y)
        {
            //���ʿ� �÷��̾ �ִ�.
            monsterSpr.flipX = false;
        }

        //�̵��� �ϸ� �̵����ð� ���ݽ����� �ʱ�ȭ�ȴ�.
        attackStack = 0;
        moveStack = 0;

        //�̵���ġ ����
        monsterMgr.AddMoveToPos(gamePos.x, gamePos.y);
        pos = gamePos;

        //�������� �̵����
        Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
        StartCoroutine(MyLib.Action2D.MoveTo(transform, toPos, 0.2f));
    }

    public override void Hit(uint pDamage, bool pCritical)
    {
        base.Hit(pDamage, pCritical);

        if (hp == 0)
        {
            dieSE.PlaySE();
        }
        else
        {
            animator.SetTrigger("Hit");
            hitSE.PlaySE();
        }
    }

}
