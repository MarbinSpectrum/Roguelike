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
    private SoundObj hurtSE;

    [SerializeField]
    private Dictionary<ButtonInput, Transform> bulletPos = new Dictionary<ButtonInput, Transform>();

    private ButtonInput ButtonInput = ButtonInput.None;
    private IEnumerator hitCor;
    private uint reloadStack = uint.MaxValue;
    private const float aniTime = 0.3f;

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
        StartCoroutine(runCatEvent());
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
        ChestManager chestManager = ChestManager.instance;

        if (mapManager.IsWall(pX, pY))
        {
            return false;
        }
        if (monsterManager.IsMonster(pX, pY))
        {
            return false;
        }
        if(jarManager.IsJar(pX,pY))
        {
            return false;
        }
        if (chestManager.IsChest(pX, pY))
        {
            return false;
        }
        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ĳ���� ���ۿ� ���� �̺�Ʈ�� ó���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCatEvent()
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
        ChestManager chestManager = ChestManager.instance;
        InventoryManager inventoryManager = InventoryManager.instance;

        ItemObjData nowWeapon = inventoryManager.NowWeapon();

        yield return new WaitUntil(()=> (buttonInput != ButtonInput.None)); //��ư �Է��� ���� ���¸� ���

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

        MonsterObj targetMonster = null;
        for (int i = 1; i <= weaponRange && targetMonster == null; i++)
        {
            //�̵� ���⿡ ���Ͱ� �����ϴ��� �˻��Ѵ�.
            //���ݹ����ȿ� ���Ͱ� ������ �ش� ��ü�� �����´�.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
            {
                //���߿� ���� ������. Ž������
                break;
            }
            if (targetMonster == null)
            {
                //���� �߰�
                targetMonster = monsterManager.IsMonster(cPos.x, cPos.y);
            }
        }

        Jar jarObj = null;
        for (int i = 1; i <= weaponRange && jarObj == null; i++)
        {
            //�̵� ���⿡ �׾Ƹ� ��ü�� �����ϴ��� �˻��Ѵ�.
            //���ݹ����ȿ� �׾Ƹ��� ������ �ش� ��ü�� �����´�.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
            {
                //���߿� ���� ������. Ž������
                break;
            }
            if (jarObj == null)
            {
                //�׾Ƹ� �߰�
                jarObj = jarManager.GetJarObj(cPos);
            }
        }

        Chest chestObj = null;
        {
            //�̵� ���⿡ ���� ��ü�� �����ϴ��� �˻��Ѵ�.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x, pos.y + dic.y);
            if (chestObj == null)
            {
                //���� �߰�
                chestObj = chestManager.GetChestObj(cPos);
            }
        }

        //���繫�Ⱑ �������� ���� Ȯ��
        bool nowShotGun = ItemManager.IsShotGun(nowWeapon.itemData.item);

        //���� ������ Ȯ��
        //reloadStack�� reloadDelay�� �Ѿ�� ���ݰ����ϴ�.
        //reloadStack�� �÷��̾ �ٸ� ������ �Ҷ����� ���δ�.
        //�ش� ������� ������ �����̸� �־���.
        uint reloadDelay = nowWeapon.itemData.reloadDelay;
        bool canFire = false;
        if (reloadDelay <= reloadStack)
        {
            //�߻簡��
            canFire = true;
        }
        else
        {
            //������ ��ä���.
            reloadStack++;
        }

        if (targetMonster != null)
        {
            if (canFire)
            {
                //���͸� �߰��߰� �߻簡���� ����

                reloadStack = 0; //���������� 0�����Ѵ�.

                float gameDis = Vector2.Distance(pos, targetMonster.pos);
                float duration = 0.1f * gameDis;

                Vector3 to = new Vector3(
                      dic.x == 0 ? bPos.x : targetMonster.pos.x * CreateMap.tileSize
                    , dic.y == 0 ? bPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

                bulletManager.FireBullet(bPos, to, duration);

                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("attack");
                animator.SetBool("shotGun", nowShotGun);
                if(nowShotGun)
                    CameraVibrate.Vibrate(10, 0.1f, 0.15f);

                yield return new WaitForSeconds(duration);

                int totalDamage = characterManager.GetTotalDamage();
                bool critical = characterManager.CriticalProcess(ref totalDamage);

                targetMonster.Hit((uint)totalDamage, critical);

                float remainTime = Mathf.Max(0, aniTime - duration);
                yield return new WaitForSeconds(remainTime);
            }
            else
            {
                //���� �Ұ����̴�.
                //���� �ִϸ��̼� ����
                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("reload");
                yield return new WaitForSeconds(aniTime);
            }

            StartCoroutine(monsterManager.RunMonster());

        }
        else if(chestObj != null)
        {
            chestObj.RemoveChestObj();

            yield return new WaitForSeconds(aniTime);
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
                animator.SetBool("shotGun", nowShotGun);
                if (nowShotGun)
                    CameraVibrate.Vibrate(10, 0.1f, 0.15f);
                else
                    CameraVibrate.Vibrate(3, 0.02f, 0.15f);

                yield return new WaitForSeconds(duration);

                int totalDamage = characterManager.GetTotalDamage();

                //damageEffect.DamageEffectRun(jarObj.pos, totalDamage, false);

                jarObj.RemoveJarObj();

                float remainTime = Mathf.Max(0, aniTime - duration);
                yield return new WaitForSeconds(remainTime);
            }
            else
            {
                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("reload");
                yield return new WaitForSeconds(aniTime);
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

        //��ȣ�� ���� ����ߴٸ�
        //����Ʈ�� �Բ� ��ȣ�� ���� �����ش�.
        characterManager.UseGuardianRing();

        characterManager.LevelUpCheck(); //�������� �������� �˻�.

        ButtonInput = ButtonInput.None;

        StartCoroutine(runCatEvent());
    }

    public void HitAni()
    {
        if (hitCor != null)
        {
            StopCoroutine(hitCor);
            hitCor = null;
        }
        hurtSE.PlaySE();
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
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        Sprite sprite = nowWeapon.itemData.frontSprite;
        gunSprite.sprite = sprite;
    }

    public void GunSpriteSide()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        Sprite sprite = nowWeapon.itemData.sideSprite;
        gunSprite.sprite = sprite;
    }
}
