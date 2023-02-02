using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl ĳ������ ��ũ��Ʈ
////////////////////////////////////////////////////////////////////////////////
public class CatGirl : SerializedMonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer gunSprite;
    [SerializeField]
    private Transform gunBase;

    [SerializeField]
    private float moveSpeed = 0.5f;
    [SerializeField]
    private Material baseMaterial;
    [SerializeField]
    private Material hitMaterial;
    [SerializeField]
    private Dictionary<ButtonInput, Transform> bulletPos = new Dictionary<ButtonInput, Transform>();

    private ButtonInput ButtonInput = ButtonInput.None;
    private IEnumerator hitCor;
    private uint reloadStack = uint.MaxValue;

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
    /// : �÷��̾��� ��ġ�� �������ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(int pX, int pY)
    {
        pos.x = pX;
        pos.y = pY;
        transform.position = new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);

        //�̴ϸ� ����
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //�ش� ��ġ�� ����� Ȱ��ȭ�Ѵ�.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        TorchManager.instance.ActAreaTorch(pos.x, pos.y);
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾��� ��ġ�� �޴´�.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetPos()
    {
        return pos;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : pX, pY�� �̵��� �� �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    private bool CanMove(int pX,int pY)
    {
        MapManager mapManager = MapManager.instance;
        MonsterManager monsterManager = MonsterManager.instance;
        JarManager jarManager = JarManager.instance;

        if(jarManager.IsJar(pX,pY))
        {
            return false;
        }
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
        ItemObjData nowWeapon = characterManager.NowWeapon();

        yield return new WaitUntil(()=> (buttonInput != ButtonInput.None));

        yield return new WaitUntil(() => (characterManager.canControl == true)); //��Ʈ�� �Ұ� �����ϰ�� ���

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

        uint weaponRange = nowWeapon.itemData.range;

        //�̵� ���⿡ ���Ͱ� �����ϴ��� �˻��Ѵ�.
        MonsterObj targetMonster = null;
        for (int i = 1; i <= weaponRange && targetMonster == null; i++)
        {
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
                break;
            if (targetMonster == null)
                targetMonster = monsterManager.IsMonster(cPos.x, cPos.y);
        }

        Jar jarObj = null;
        for (int i = 1; i <= weaponRange && jarObj == null; i++)
        {
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
                break;
            if (jarObj == null)
                jarObj = jarManager.GetJarObj(cPos);
        }

        //���� ������ Ȯ��
        uint reloadDelay = nowWeapon.itemData.reloadDelay;
        bool canFire = false;
        if (reloadDelay <= reloadStack)
            canFire = true;
        else
            reloadStack++;

        if (targetMonster != null)
        {
            if (canFire)
            {
                reloadStack = 0;

                float gameDis = Vector2.Distance(pos, targetMonster.pos);
                float duration = 0.1f * gameDis;

                Vector3 to = new Vector3(
                      dic.x == 0 ? bPos.x : targetMonster.pos.x * CreateMap.tileSize
                    , dic.y == 0 ? bPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

                bulletManager.FireBullet(bPos, to, duration);

                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("attack");

                yield return new WaitForSeconds(duration);

                int totalDamage = characterManager.GetTotalDamage();
                bool critical = characterManager.CriticalProcess(ref totalDamage);

                targetMonster.Hit((uint)totalDamage, critical);
            }
            else
            {
                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("reload");
                yield return new WaitForSeconds(0.2f);
            }

            StartCoroutine(monsterManager.RunMonster());

        }
        else if (jarObj != null)
        {
            //�̵� �������� �׾Ƹ��� �����Ѵ�.
            //�׾Ƹ��� �μ���.

            if (canFire)
            {
                reloadStack = 0;

                float gameDis = Vector2.Distance(pos, jarObj.pos);
                float duration = 0.1f * gameDis;

                Vector3 to = new Vector3(
                      dic.x == 0 ? bPos.x : jarObj.pos.x * CreateMap.tileSize
                    , dic.y == 0 ? bPos.y : jarObj.pos.y * CreateMap.tileSize, 0);

                bulletManager.FireBullet(bPos, to, duration);

                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("attack");

                yield return new WaitForSeconds(duration);

                int totalDamage = characterManager.GetTotalDamage();

                damageEffect.DamageEffectRun(jarObj.pos, totalDamage, false);

                jarObj.RemoveJarObj();
            }
            else
            {
                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("reload");
                yield return new WaitForSeconds(0.2f);
            }

            StartCoroutine(monsterManager.RunMonster());
        }
        else if (CanMove(pos.x + dic.x, pos.y + dic.y))
        {
            //�ش� �������� �̵��������� �˻��Ѵ�.
            //�̵������ϸ� �̵��Ѵ�.
            pos.x += dic.x;
            pos.y += dic.y;
            spriteRenderer.flipX = spriteFiipX;
            gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
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

        //�ش� ��ġ�� ����� Ȱ��ȭ�Ѵ�.
        mapManager.ActAreaTile(pos.x, pos.y);

        //�ش� ��ġ�� ��ġ�� Ȱ��ȭ��Ų��.
        torchManager.ActAreaTorch(pos.x, pos.y);

        //�̵� ��ġ�� �������� �����ϴ��� Ȯ���Ѵ�.
        ItemObj itemObj = itemManager.GetItem(pos.x, pos.y);
        if(itemObj != null)
        {
            //�����ϸ� �������� ��´�.
            itemObj.GetItem();
        }

        //�̴ϸ� ����
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

    public void GunSpriteFront()
    {
        CharacterManager characterManager = CharacterManager.instance;
        ItemObjData nowWeapon = characterManager.NowWeapon();
        Sprite sprite = nowWeapon.itemData.frontSprite;
        gunSprite.sprite = sprite;
    }

    public void GunSpriteSide()
    {
        CharacterManager characterManager = CharacterManager.instance;
        ItemObjData nowWeapon = characterManager.NowWeapon();
        Sprite sprite = nowWeapon.itemData.sideSprite;
        gunSprite.sprite = sprite;
    }
}
