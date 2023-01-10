using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////////////////////////////////////////////////////////////////////////////
/// : 몬스터 오브젝트의 원형
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


    ////////////////////////////////////////////////////////////////////////////////
    /// : pMonsterData기반으로 몬스터를 초기화
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
    /// : 몬스터의 위치를 설정해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(Vector2Int pPos)
    {
        pos = pPos;
        transform.position = new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터의 행동을 처리한다.
    ////////////////////////////////////////////////////////////////////////////////
    public virtual void RunMonster(List<Vector2Int> pRoute)
    {
        if (pRoute == null || pRoute.Count == 0)
            return;

        MonsterManager monsterManager = MonsterManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        MapManager mapManager = MapManager.instance;

        if (sleep)
        {
            if (range > pRoute.Count)
            {
                //플레이어가 인식범위까지 들어왔다.
                //잠에서 깨어난다.
                sleep = false;
            }
            return;
        }

        Vector2Int gamePos = pRoute[pRoute.Count - 1];
        if (monsterManager.IsMonster(gamePos.x, gamePos.y))
        {
            //해당위치에 몬스터가 있다. 이동하지 않는다.
            return;
        }
        if (monsterManager.IsMoveToPos(gamePos.x, gamePos.y))
        {
            //해당위치에 몬스터가 이동하기로 했다. 이동하지 않는다.
            return;
        }
        if (mapManager.IsWall(gamePos.x, gamePos.y))
        {
            //해당위치는 벽이다 이동하지 않는다.
            return;
        }
        if (gamePos == characterManager.character.GetPos())
        {
            //캐릭터가 공격범위에 있다.
            //이동하지않고 플레이어를 공격한다.













            //공격시 이동스택은 초기화된다.
            moveStack = 0;
            return;
        }
        if(moveStack < moveDelay)
        {
            //이동스택이 다 쌓인후 이동 한다.
            moveStack++;
            return;
        }

        //이동을 하면 이동스택은 초기화된다.
        moveStack = 0;

        //이동위치 갱신
        monsterManager.AddMoveToPos(gamePos.x,gamePos.y);
        pos = gamePos;

        //실질적인 이동명령
        Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
        StartCoroutine(MyLib.Action2D.MoveTo(transform, toPos, 0.2f));
    }
}
