using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////////////////////////////////////////////////////////////////////////////
/// : 몬스터 오브젝트의 원형
////////////////////////////////////////////////////////////////////////////////
public abstract class MonsterObj : MonoBehaviour
{
    //체력
    public uint hp;

    //데미지
    public uint damage;

    //공격딜레이
    public uint attackDelay;
    public uint attackStack;

    //이동딜레이
    public uint moveDelay;
    public uint moveStack;

    //인식범위
    public uint range;

    //처치시 얻는 경험치
    public uint exp;

    public Vector2Int pos;
    public bool alive;
    public bool sleep = true;
    public bool stun;

    public SpriteRenderer monsterSpr;
    public Material baseMaterial;
    public Material hitMaterial;
    private IEnumerator hitCor;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pMonsterData기반으로 몬스터를 초기화
    ////////////////////////////////////////////////////////////////////////////////
    public void Init(MonsterData pMonsterData)
    {
        sleep = true;
        alive = true;

        hp = pMonsterData.hp;
        damage = pMonsterData.damage;
        range = pMonsterData.range;
        moveDelay = pMonsterData.moveDelay;
        attackDelay = pMonsterData.attackDelay;
        exp = pMonsterData.exp;
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
        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;

        if (alive == false)
        {
            monsterSpr.enabled = false;
            return;
        }

        if(stun)
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
        if (chestManager.IsChest(gamePos.x, gamePos.y))
        {
            //상자는 부술수없다. 이동하지 않는다.
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












            //공격시 이동스택과 공격스택이 초기화된다.
            attackStack = 0;
            moveStack = 0;
            return;
        }
        if(moveStack < moveDelay)
        {
            //이동스택이 다 쌓인후 이동 한다.
            moveStack++;
            return;
        }

        if (jarManager.IsJar(gamePos.x, gamePos.y))
        {
            //이동위치에 항아리가 있으면 항아리를 부순다.
            Jar jarObj = jarManager.GetJarObj(gamePos);
            jarObj.RemoveJarObj();
        }

        //이동을 하면 이동스택과 공격스택은 초기화된다.
        attackStack = 0;
        moveStack = 0;

        //이동위치 갱신
        monsterManager.AddMoveToPos(gamePos.x,gamePos.y);
        pos = gamePos;

        //실질적인 이동명령
        Vector3 toPos = new Vector3(gamePos.x * CreateMap.tileSize, gamePos.y * CreateMap.tileSize, 0);
        StartCoroutine(MyLib.Action2D.MoveTo(transform, toPos, 0.2f));
    }

    public virtual void Hit(uint pDamage, bool pCritical)
    {
        stun = true;
        moveStack = 0;

        DamageEffect damageEffect = DamageEffect.instance;
        damageEffect.DamageEffectRun(pos, (int)pDamage, pCritical);

        if (hp < pDamage)
            hp = 0;
        else
        {
            if(hitCor != null)
            {
                StopCoroutine(hitCor);
                hitCor = null;
            }
            hitCor = HitStunAni();
            StartCoroutine(hitCor);

            hp -= pDamage;
        }

        if(hp == 0)
        {
            alive = false;
            CharacterManager characterManager = CharacterManager.instance;
            characterManager.GetExp(exp);
        }
    }

    public virtual IEnumerator HitStunAni()
    {
        monsterSpr.material = hitMaterial;
        yield return new WaitForSeconds(0.05f);
        monsterSpr.material = baseMaterial;
    }

    public virtual void PlayerHitAni()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.character.HitAni();
    }
}
