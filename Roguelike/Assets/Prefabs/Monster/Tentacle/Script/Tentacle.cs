using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonsterObj
{
    [SerializeField]
    private Animator animator;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터의 행동을 처리한다.
    ////////////////////////////////////////////////////////////////////////////////
    public override void RunMonster(List<Vector2Int> pRoute)
    {
        if (pRoute == null || pRoute.Count == 0)
            return;

        MonsterManager monsterManager = MonsterManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        MapManager mapManager = MapManager.instance;
        JarManager jarManager = JarManager.instance;

        if (alive == false)
        {
            monsterSpr.enabled = false;
            return;
        }

        if (stun)
        {
            //스턴 상태이다.
            //스턴상태에서 깨어난다.
            //깨어나면서 이동스택과 공격스택이 초기화된다.
            attackStack = 0;
            moveStack = 0;

            stun = false;
            return;
        }

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
            if (attackStack < attackDelay)
            {
                //공격스택이 다 쌓인후 이동 한다.
                attackStack++;
                return;
            }

            //캐릭터가 공격범위에 있다.
            //이동하지않고 플레이어를 공격한다.
            animator.SetTrigger("Attack");

            if (pos.x + 1 == gamePos.x)
            {
                //오른쪽에 플레이어가 있다.
                monsterSpr.flipX = false;
            }
            else if (pos.x - 1 == gamePos.x)
            {
                //왼쪽에 플레이어가 있다.
                monsterSpr.flipX = true;
            }
            else if (pos.x - 1 == gamePos.y)
            {
                //아래쪽에 플레이어가 있다.
                monsterSpr.flipX = false;
            }
            else if (pos.x + 1 == gamePos.y)
            {
                //위쪽에 플레이어가 있다.
                monsterSpr.flipX = false;
            }

            characterManager.Hit(damage);





            //공격시 이동스택과 공격스택이 초기화된다.
            attackStack = 0;
            moveStack = 0;
            return;
        }
        if (moveStack < moveDelay)
        {
            //이동스택이 다 쌓인후 이동 한다.
            moveStack++;
            return;
        }

        if(jarManager.IsJar(gamePos.x, gamePos.y))
        {
            //이동위치에 항아리가 있으면 항아리를 부순다.
            Jar jarObj = jarManager.GetJarObj(gamePos);
            jarObj.RemoveJarObj();
        }

        //이동을 하면 이동스택과 공격스택은 초기화된다.
        attackStack = 0;
        moveStack = 0;

        //이동위치 갱신
        monsterManager.AddMoveToPos(gamePos.x, gamePos.y);
        pos = gamePos;

        //실질적인 이동명령
        Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
        StartCoroutine(MyLib.Action2D.MoveTo(transform, toPos, 0.2f));
    }

}
