using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////////////////////////////////////////////////////////////////////////////
/// : ���� ������Ʈ�� ����
////////////////////////////////////////////////////////////////////////////////
public abstract class MonsterObj : MonoBehaviour
{
    public uint hp;
    public uint damage;
    public uint range;
    public uint moveDelay;
    public uint moveStack;

    public Vector2Int pos;
    public bool alive;
    public bool sleep = true;

    public SpriteRenderer monsterSpr;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pMonsterData������� ���͸� �ʱ�ȭ
    ////////////////////////////////////////////////////////////////////////////////
    public void Init(MonsterData pMonsterData)
    {
        Init(pMonsterData.hp, pMonsterData.damage, pMonsterData.range, pMonsterData.moveDelay);
    }
    public void Init(uint pHp, uint pDamage, uint pRange, uint pMoveDelay)
    {
        sleep = true;
        alive = true;

        hp = pHp;
        damage = pDamage;
        range = pRange;
        moveDelay = pMoveDelay;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ ��ġ�� �������ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(Vector2Int pPos)
    {
        pos = pPos;
        transform.position = new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �ൿ�� ó���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public virtual void RunMonster(List<Vector2Int> pRoute)
    {
        if (pRoute == null || pRoute.Count == 0)
            return;

        MonsterManager monsterManager = MonsterManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        MapManager mapManager = MapManager.instance;

        if(alive == false)
        {
            monsterSpr.enabled = false;
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













            //���ݽ� �̵������� �ʱ�ȭ�ȴ�.
            moveStack = 0;
            return;
        }
        if(moveStack < moveDelay)
        {
            //�̵������� �� ������ �̵� �Ѵ�.
            moveStack++;
            return;
        }

        //�̵��� �ϸ� �̵������� �ʱ�ȭ�ȴ�.
        moveStack = 0;

        //�̵���ġ ����
        monsterManager.AddMoveToPos(gamePos.x,gamePos.y);
        pos = gamePos;

        //�������� �̵����
        Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
        StartCoroutine(MyLib.Action2D.MoveTo(transform, toPos, 0.2f));
    }

    public virtual void Hit(uint pDamage)
    {
        if (hp < pDamage)
            hp = 0;
        else
            hp -= pDamage;

        if(hp == 0)
            alive = false;
    }
}
