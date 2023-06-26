using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl ĳ������ ��ũ��Ʈ
////////////////////////////////////////////////////////////////////////////////
public class CatGirl : Mgr
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer gunSprite;
    [SerializeField]
    private Transform effectBase;
    [SerializeField]
    private Transform gunBase;

    [SerializeField]
    private float moveDuration = 0.5f;
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

        torchMgr.ActAreaTorch(pos.x, pos.y);
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

        if (mapManager.IsWall(pX, pY))
        {
            return false;
        }
        if (monsterMgr.IsMonster(pX, pY))
        {
            return false;
        }
        if(jarMgr.IsJar(pX,pY))
        {
            return false;
        }
        if (chestMgr.IsChest(pX, pY))
        {
            return false;
        }
        if (mapManager.CanMoveGunBench(pos, new Vector2Int(pX, pY)) == false)
        {
            return false;
        }
        if(mapManager.CanMoveShop(pos, new Vector2Int(pX, pY)) == false)
        {
            return false;
        }
        if (mapManager.CantMovePos(new Vector2Int(pX, pY)))
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
        MapManager mapManager = MapManager.instance;
        TotalUI totalUI = TotalUI.instance;

        GetGoldEffect getGoldEffect = uIEffectMgr.getGoldEffect;
        DamageEffect damageEffect = uIEffectMgr.damageEffect;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();

        yield return new WaitUntil(()=> (buttonInput != ButtonInput.None)); //��ư �Է��� ���� ���¸� ���

        yield return new WaitUntil(() => (characterMgr.canControl == true)); //��Ʈ�� �Ұ� �����ϰ�� ���

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

        bool useKnife = false;
        if(characterMgr.nowBullet == 0)
        {
            //�Ѿ��� �ٴڳ���. �ܰ��÷��̸� ���ؼ�
            //�÷��׸� �����Ѵ�.
            useKnife = true;
        }

        int frontShowDic = animator.GetInteger("showDic");
        animator.SetInteger("showDic", showDic);

        uint weaponRange = nowWeapon.itemData.range;
        if(useKnife)
        {
            //�Ѿ��� �ٴڳ���. �ܰ����� �÷����Ѵ�.
            //�ܰ��� ��Ÿ��� 1
            weaponRange = 1;
        }

        MonsterObj targetMonster = null;
        Jar jarObj = null;
        Chest chestObj = null;
        for (int i = 1; i <= weaponRange; i++)
        {
            //�̵� ���⿡ ������Ʈ�� �����ϴ��� �˻��Ѵ�.
            //���ݹ����ȿ� ������Ʈ�� ������ �ش� ��ü�� �����´�.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
            {
                //���߿� ���� ������. Ž������
                break;
            }

            targetMonster = monsterMgr.IsMonster(cPos.x, cPos.y);
            if (targetMonster != null)
            {
                //���� �߰�
                break;
            }


            if (i == 1)
            {
                jarObj = jarMgr.GetJarObj(cPos);
                if (jarObj != null)
                {
                    //�׾Ƹ� �߰�
                    //�Ÿ��� 1�ۿ��ȵǸ� Į�� �μ���.
                    useKnife = true;
                    break;

                }

                chestObj = chestMgr.GetChestObj(cPos);
                if (chestObj != null)
                {
                    //���� �߰�
                    break;
                }
            }
        }

        animator.SetBool("useKnife", useKnife);

        //���繫�Ⱑ �������� ���� Ȯ��
        bool nowShotGun = ItemManager.IsShotGun(nowWeapon.itemData.item);

        //���� ������ Ȯ��
        //reloadStack�� reloadDelay�� �Ѿ�� ���ݰ����ϴ�.
        //reloadStack�� �÷��̾ �ٸ� ������ �Ҷ����� ���δ�.
        //�ش� ������� ������ �����̸� �־���.
        uint reloadDelay = nowWeapon.itemData.reloadDelay;
        if (useKnife)
        {
            //�Ѿ��� �ٴڳ���. �ܰ����� �÷����Ѵ�.
            //���ݵ����̴� 2
            reloadDelay = 2;
        }

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
                //���͸� �߰��߰� ������ ������ ����

                reloadStack = 0; //��������(���ݽ���)�� 0�����Ѵ�.

                if (useKnife)
                {
                    //�ܰ����� ����
                    spriteRenderer.flipX = spriteFiipX;
                    effectBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                    animator.SetTrigger("attack");
                    animator.SetBool("shotGun", nowShotGun);

                    int totalDamage = 1;
                    bool critical = false;

                    yield return new WaitForSeconds(0.1f);

                    targetMonster.Hit((uint)totalDamage, critical);

                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    float gameDis = Vector2.Distance(pos, targetMonster.pos);
                    float duration = 0.075f * gameDis;

                    Vector3 to = new Vector3(
                          dic.x == 0 ? bPos.x : targetMonster.pos.x * CreateMap.tileSize,
                          dic.y == 0 ? bPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

                    to += new Vector3(
                        dic.x == 0 ? Random.Range(-1, 1) : 0,
                        dic.y == 0 ? Random.Range(-1, 1) : 0) * 0.2f;

                    bulletMgr.FireBullet(bPos, to, duration);

                    spriteRenderer.flipX = spriteFiipX;
                    gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                    animator.SetTrigger("attack");
                    animator.SetBool("shotGun", nowShotGun);

                    if (nowShotGun)
                        CameraVibrate.Vibrate(10, 0.1f, 0.15f);
                    characterMgr.CostBullet();

                    yield return new WaitForSeconds(duration);

                    int totalDamage = characterMgr.GetTotalDamage();
                    bool critical = characterMgr.CriticalProcess(ref totalDamage);

                    targetMonster.Hit((uint)totalDamage, critical);

                    float remainTime = Mathf.Max(0, aniTime - duration);
                    yield return new WaitForSeconds(remainTime);
                }
            }
            else
            {
                if (useKnife)
                {
                    //���� �Ұ����̴�.
                    //�ܰ˻����̹Ƿ� ���
                    spriteRenderer.flipX = spriteFiipX;
                    animator.SetTrigger("idle");
                    yield return new WaitForSeconds(aniTime);
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
            }

            StartCoroutine(monsterMgr.RunMonster());

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
                if (useKnife)
                {
                    //�ܰ����� ����
                    spriteRenderer.flipX = spriteFiipX;
                    effectBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                    animator.SetTrigger("attack");
                    animator.SetBool("shotGun", nowShotGun);

                    yield return new WaitForSeconds(0.1f);

                    jarObj.RemoveJarObj();

                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    float gameDis = Vector2.Distance(pos, jarObj.pos);
                    float duration = 0.1f * gameDis;

                    Vector3 to = new Vector3(
                          dic.x == 0 ? bPos.x : jarObj.pos.x * CreateMap.tileSize
                        , dic.y == 0 ? bPos.y : jarObj.pos.y * CreateMap.tileSize, 0);

                    bulletMgr.FireBullet(bPos, to, duration);

                    spriteRenderer.flipX = spriteFiipX;
                    gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                    animator.SetTrigger("attack");
                    animator.SetBool("shotGun", nowShotGun);

                    if (nowShotGun)
                        CameraVibrate.Vibrate(10, 0.1f, 0.15f);
                    characterMgr.CostBullet();

                    yield return new WaitForSeconds(duration);

                    jarObj.RemoveJarObj();

                    float remainTime = Mathf.Max(0, aniTime - duration);
                    yield return new WaitForSeconds(remainTime);
                }
            }
            else
            {
                if (useKnife)
                {
                    //���� �Ұ����̴�.
                    //�ܰ˻����̹Ƿ� ���
                    spriteRenderer.flipX = spriteFiipX;
                    animator.SetTrigger("idle");
                    yield return new WaitForSeconds(aniTime);
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
            }

            StartCoroutine(monsterMgr.RunMonster());
        }
        else if (CanMove(pos.x + dic.x, pos.y + dic.y))
        {
            //�ش� �������� �̵��������� �˻��Ѵ�.
            //�̵������ϸ� �̵��Ѵ�.
            pos.x += dic.x;
            pos.y += dic.y;
            spriteRenderer.flipX = spriteFiipX;
            gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
            StartCoroutine(monsterMgr.RunMonster());
            animator.SetTrigger("run");
            Vector3 to = transform.position + new Vector3(dic.x * CreateMap.tileSize, dic.y * CreateMap.tileSize, 0);
            yield return Action2D.MoveTo(transform, to, moveDuration);
            animator.SetTrigger("idle");
        }
        else //if(frontShowDic != showDic)
        {
            spriteRenderer.flipX = spriteFiipX;
            gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
            yield return monsterMgr.RunMonster();
            //yield return new WaitForSeconds(aniTime);
            if (frontShowDic != showDic)
            {
                animator.SetTrigger("idle");
            }
        }

        //Ʈ�� �۵�
        trapMgr.RunTrap();

        //�ش� ��ġ�� ����� Ȱ��ȭ�Ѵ�.
        mapManager.ActAreaTile(pos.x, pos.y);

        //�ش� ��ġ�� ��ġ�� Ȱ��ȭ��Ų��.
        torchMgr.ActAreaTorch(pos.x, pos.y);

        //�̵� ��ġ�� �������� �����ϴ��� Ȯ���Ѵ�.
        ItemObj itemObj = itemMgr.GetItem(pos.x, pos.y);
        if(itemObj != null)
        {
            //�����ϸ� �������� ��´�.
            itemObj.GetItem();
        }

        //�̴ϸ� ����
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //��ȣ�� ���� ����ߴٸ�
        //����Ʈ�� �Բ� ��ȣ�� ���� �����ش�.
        characterMgr.UseGuardianRing();

        characterMgr.LevelUpCheck(); //�������� �������� �˻�.

        ButtonInput = ButtonInput.None;

        if(mapManager.GetEndPos() == pos)
        {
            //Ż�ⱸ �߰�
            //���� ������ �̵��Ѵ�.
            yield return StageManager.LoadNextMap();
            yield break;
        }
        else if (mapManager.GetGunBenchPos() == pos && GameManager.gunBenchAct)
        {
            //�ѱ� �۾��� �߰�
            //�ѱ� �۾��� UI����
            totalUI.ActGunBench(true);
        }
        else if (mapManager.GetShopPos() == pos)
        {
            //���� �߰�
            //���� UI����
            totalUI.ActShop(true);
        }

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
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        Sprite sprite = nowWeapon.itemData.frontSprite;
        gunSprite.sprite = sprite;
    }

    public void GunSpriteSide()
    {
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        Sprite sprite = nowWeapon.itemData.sideSprite;
        gunSprite.sprite = sprite;
    }
}
