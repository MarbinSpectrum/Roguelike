using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonsterObj
{
    [SerializeField]
    private Animator animator;

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �ൿ�� ó���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public override void RunMonster(List<Vector2Int> pRoute)
    {
        if (pRoute == null || pRoute.Count == 0)
            return;

        MonsterManager monsterManager = MonsterManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
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
                sleep = false;
            }
            return;
        }

        Vector2Int gamePos = pRoute[pRoute.Count - 1];
        if (monsterManager.IsMonster(gamePos.x, gamePos.y))
        {
            //�ش���ġ�� ���Ͱ� �ִ�. �̵����� �ʴ´�.
            return;
        }
        if (monsterManager.IsMoveToPos(gamePos.x, gamePos.y))
        {
            //�ش���ġ�� ���Ͱ� �̵��ϱ�� �ߴ�. �̵����� �ʴ´�.
            return;
        }
        if (mapManager.IsWall(gamePos.x, gamePos.y))
        {
            //�ش���ġ�� ���̴� �̵����� �ʴ´�.
            return;
        }
        if (gamePos == characterManager.character.GetPos())
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
            animator.SetTrigger("Attack");

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
            else if (pos.x - 1 == gamePos.y)
            {
                //�Ʒ��ʿ� �÷��̾ �ִ�.
                monsterSpr.flipX = false;
            }
            else if (pos.x + 1 == gamePos.y)
            {
                //���ʿ� �÷��̾ �ִ�.
                monsterSpr.flipX = false;
            }

            characterManager.Hit(damage);





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

        //�̵��� �ϸ� �̵����ð� ���ݽ����� �ʱ�ȭ�ȴ�.
        attackStack = 0;
        moveStack = 0;

        //�̵���ġ ����
        monsterManager.AddMoveToPos(gamePos.x, gamePos.y);
        pos = gamePos;

        //�������� �̵����
        Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
        StartCoroutine(MyLib.Action2D.MoveTo(transform, toPos, 0.2f));
    }

}
