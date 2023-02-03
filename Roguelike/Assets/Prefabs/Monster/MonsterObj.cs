using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////////////////////////////////////////////////////////////////////////////
/// : ���� ������Ʈ�� ����
////////////////////////////////////////////////////////////////////////////////
public abstract class MonsterObj : MonoBehaviour
{
    //ü��
    public uint hp;

    //������
    public uint damage;

    //���ݵ�����
    public uint attackDelay;
    public uint attackStack;

    //�̵�������
    public uint moveDelay;
    public uint moveStack;

    //�νĹ���
    public uint range;

    //óġ�� ��� ����ġ
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
    /// : pMonsterData������� ���͸� �ʱ�ȭ
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
        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;

        if (alive == false)
        {
            monsterSpr.enabled = false;
            return;
        }

        if(stun)
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
        if (chestManager.IsChest(gamePos.x, gamePos.y))
        {
            //���ڴ� �μ�������. �̵����� �ʴ´�.
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












            //���ݽ� �̵����ð� ���ݽ����� �ʱ�ȭ�ȴ�.
            attackStack = 0;
            moveStack = 0;
            return;
        }
        if(moveStack < moveDelay)
        {
            //�̵������� �� ������ �̵� �Ѵ�.
            moveStack++;
            return;
        }

        if (jarManager.IsJar(gamePos.x, gamePos.y))
        {
            //�̵���ġ�� �׾Ƹ��� ������ �׾Ƹ��� �μ���.
            Jar jarObj = jarManager.GetJarObj(gamePos);
            jarObj.RemoveJarObj();
        }

        //�̵��� �ϸ� �̵����ð� ���ݽ����� �ʱ�ȭ�ȴ�.
        attackStack = 0;
        moveStack = 0;

        //�̵���ġ ����
        monsterManager.AddMoveToPos(gamePos.x,gamePos.y);
        pos = gamePos;

        //�������� �̵����
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
