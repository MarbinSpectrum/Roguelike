using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl 캐릭터의 스크립트
////////////////////////////////////////////////////////////////////////////////
public class CatGirl : SerializedMonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float moveSpeed = 0.5f;
    [SerializeField]
    private int attackRange = 3;
    [SerializeField]
    private Material baseMaterial;
    [SerializeField]
    private Material hitMaterial;
    [SerializeField]
    private Dictionary<ButtonInput, Transform> bulletPos = new Dictionary<ButtonInput, Transform>();

    private ButtonInput ButtonInput = ButtonInput.None;
    private IEnumerator hitCor;

    [HideInInspector]
    public ButtonInput buttonInput
    {
        set
        {
            if(ButtonInput == ButtonInput.None)
            {
                ButtonInput = value;
            }
        }
        get
        {
            return ButtonInput;
        }
    }

    private Vector2Int pos;

    ////////////////////////////////////////////////////////////////////////////////
    /// : Start
    ////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        StartCoroutine(ButtonEventDelay());
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어의 위치를 지정해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(int pX, int pY)
    {
        pos.x = pX;
        pos.y = pY;
        transform.position = new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);

        //미니맵 갱신
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //해당 위치의 블록을 활성화한다.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        TorchManager.instance.ActAreaTorch(pos.x, pos.y);
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어의 위치를 받는다.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetPos()
    {
        return pos;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : pX, pY로 이동할 수 있는지 확인한다.
    ////////////////////////////////////////////////////////////////////////////////
    private bool CanMove(int pX,int pY)
    {
        MapManager mapManager = MapManager.instance;
        MonsterManager monsterManager = MonsterManager.instance;

        if (mapManager.IsWall(pX, pY))
        {
            return false;
        }
        if (monsterManager.IsMonster(pX, pY))
        {
            return false;
        }
        return true;
    }

    public IEnumerator ButtonEventDelay()
    {
        MonsterManager monsterManager = MonsterManager.instance;
        MapManager mapManager = MapManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        ItemManager itemManager = ItemManager.instance;
        TotalUI totalUI = TotalUI.instance;
        TorchManager torchManager = TorchManager.instance;
        DamageEffect damageEffect = DamageEffect.instance;
        BulletManager bulletManager = BulletManager.instance;
        JarManager jarManager = JarManager.instance;

        yield return new WaitUntil(()=> (buttonInput != ButtonInput.None));

        yield return new WaitUntil(() => (characterManager.canControl == true)); //컨트롤 불가 상태일경우 대기

        int showDic = 0;
        Vector2Int dic = Vector2Int.zero;
        Vector3 bPos = bulletPos[buttonInput].position;
        bool spriteFiipX = false;

        switch (buttonInput)
        {
            case ButtonInput.Left:
                showDic = 0;
                dic = new Vector2Int(-1, 0);
                spriteFiipX = true;
                break;
            case ButtonInput.Right:
                showDic = 0;
                dic = new Vector2Int(+1, 0);
                spriteFiipX = false;
                break;
            case ButtonInput.Up:
                showDic = 1;
                dic = new Vector2Int(0, +1);
                spriteFiipX = false;
                break;
            case ButtonInput.Down:
                showDic = 2;
                dic = new Vector2Int(0, -1);
                spriteFiipX = false;
                break;
        }

        int frontShowDic = animator.GetInteger("showDic");
        animator.SetInteger("showDic", showDic);

        //이동 방향에 몬스터가 존재하는지 검사한다.
        MonsterObj targetMonster = null;
        for (int i = 1; i <= attackRange && targetMonster == null; i++)
        {
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
                break;
            if (targetMonster == null)
                targetMonster = monsterManager.IsMonster(cPos.x, cPos.y);
        }

        Jar jarObj = null;
        for (int i = 1; i <= attackRange && jarObj == null; i++)
        {
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
                break;
            if (jarObj == null)
                jarObj = jarManager.GetJarObj(cPos);
        }

        if (targetMonster != null)
        {
            //이동 방향으로 몬스터가 존재한다.
            //몬스터를 공격한다.

            float gameDis = Vector2.Distance(pos, targetMonster.pos);
            float duration = 0.1f * gameDis;

            Vector3 to = new Vector3(
                  dic.x == 0 ? bPos.x : targetMonster.pos.x * CreateMap.tileSize
                , dic.y == 0 ? bPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

            bulletManager.FireBullet(bPos, to, duration);

            spriteRenderer.flipX = spriteFiipX;
            animator.SetTrigger("attack");

            yield return new WaitForSeconds(duration);

            int totalDamage = characterManager.GetTotalDamage();
            bool critical = characterManager.CriticalProcess(ref totalDamage);

            targetMonster.Hit((uint)totalDamage, critical);

            StartCoroutine(monsterManager.RunMonster());
        }
        else if (jarObj != null)
        {
            //이동 방향으로 항아리가 존재한다.
            //항아리를 부순다.

            float gameDis = Vector2.Distance(pos, jarObj.pos);
            float duration = 0.1f * gameDis;

            Vector3 to = new Vector3(
                  dic.x == 0 ? bPos.x : jarObj.pos.x * CreateMap.tileSize
                , dic.y == 0 ? bPos.y : jarObj.pos.y * CreateMap.tileSize, 0);

            bulletManager.FireBullet(bPos, to, duration);

            spriteRenderer.flipX = spriteFiipX;
            animator.SetTrigger("attack");

            yield return new WaitForSeconds(duration);

            int totalDamage = characterManager.GetTotalDamage();

            damageEffect.DamageEffectRun(jarObj.pos, totalDamage, false);

            jarObj.RemoveJarObj();

            StartCoroutine(monsterManager.RunMonster());
        }
        else if (CanMove(pos.x + dic.x, pos.y + dic.y))
        {
            //해당 방향으로 이동가능한지 검사한다.
            //이동가능하면 이동한다.
            pos.x += dic.x;
            pos.y += dic.y;
            spriteRenderer.flipX = spriteFiipX;
            StartCoroutine(monsterManager.RunMonster());
            animator.SetTrigger("run");
            Vector3 to = transform.position + new Vector3(dic.x * CreateMap.tileSize, dic.y * CreateMap.tileSize, 0);
            yield return Action2D.MoveTo(transform, to, moveSpeed);
            animator.SetTrigger("idle");
        }
        else if(frontShowDic != showDic)
        {
            animator.SetTrigger("idle");
        }

        //해당 위치의 블록을 활성화한다.
        mapManager.ActAreaTile(pos.x, pos.y);

        //해당 위치의 토치를 활성화시킨다.
        torchManager.ActAreaTorch(pos.x, pos.y);

        //이동 위치에 아이템이 존재하는지 확인한다.
        ItemObj itemObj = itemManager.GetItem(pos.x, pos.y);
        if(itemObj != null)
        {
            //존재하면 아이템을 얻는다.
            itemObj.GetItem();
        }

        //미니맵 갱신
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);


        ButtonInput = ButtonInput.None;

        StartCoroutine(ButtonEventDelay());
    }

    public void HitAni()
    {
        if (hitCor != null)
        {
            StopCoroutine(hitCor);
            hitCor = null;
        }
        hitCor = HitStunAni();
        StartCoroutine(hitCor);
    }

    protected virtual IEnumerator HitStunAni()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material = baseMaterial;
    }
}
