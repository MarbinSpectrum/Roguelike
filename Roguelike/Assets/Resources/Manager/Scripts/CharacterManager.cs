using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : ĳ���͸� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class CharacterManager : DontDestroySingleton<CharacterManager>
{
    public Item startWeapon;
    public Item startAccessary;
    public int startHp;
    public int startPow;
    public int startBalance;
    public int startCriDamage;
    public int startCriRate;
    public int startBullet;

    [Space(40)]

    [SerializeField]
    private CatGirl catGirl;
    private CatGirl character;

    #region[public uint baseMaxHp]
    private int BaseMaxHp;
    public int baseMaxHp
    {
        get
        {
            return BaseMaxHp;
        }
    }
    #endregion

    #region[public uint nowHp]
    private int NowHp;
    public int nowHp
    {
        get
        {
            return NowHp;
        }
    }
    #endregion

    #region[public uint maxExp]
    private uint MaxExp;
    public uint maxExp
    {
        get
        {
            return MaxExp;
        }
    }
    #endregion

    #region[public uint nowExp]
    private uint NowExp;
    public uint nowExp
    {
        get
        {
            return NowExp;
        }
    }
    #endregion

    #region[public uint nowLevel]
    private uint NowLevel;
    public uint nowLevel
    {
        get
        {
            return NowLevel;
        }
    }
    #endregion

    #region[public uint basePow]
    private int BasePow;
    public int basePow
    {
        get
        {
            return BasePow;
        }
    }
    #endregion

    #region[public uint baseBalance]
    private int BaseBalance;
    public int baseBalance
    {
        get
        {
            return BaseBalance;
        }
    }
    #endregion

    private const int MAX_BALANCE = 100;

    #region[public float baseCriPer]
    private float BaseCriPer;
    public float baseCriPer
    {
        get
        {
            return BaseCriPer;
        }
    }
    #endregion

    #region[public float baseCriDamage]
    private float BaseCriDamage;
    public float baseCriDamage
    {
        get
        {
            return BaseCriDamage;
        }
    }
    #endregion

    #region[public int maxBullet]
    private int MaxBullet;
    public int maxBullet
    {
        get
        {
            return MaxBullet;
        }
    }
    #endregion

    #region[public uint nowBullet]
    private int NowBullet;
    public int nowBullet
    {
        get
        {
            return NowBullet;
        }
    }
    #endregion

    [HideInInspector]
    public bool canControl;

    public const int MAX_ACCESSARY_SLOT = 4;

    private bool use_Guardian_Ring = false;
    private int guardian_Ring_idx = 0;
    private int gudrdian_cnt = 0;

    //��ư �Է�ó��
    private ButtonInput ButtonInput = ButtonInput.None;
    [HideInInspector]
    public ButtonInput buttonInput
    {
        set
        {
            if (ButtonInput == ButtonInput.None)
            {
                ButtonInput = value;
            }
        }
        get
        {
            return ButtonInput;
        }
    }

    private uint reloadStack = uint.MaxValue;
    private Vector2Int pos;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY�� �ش��ϴ� ��ǥ�� ĳ���͸� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateCatGirl(int pX, int pY)
    {
        if (character != null)
        {
            Destroy(character);
            return;
        }

        PlayData playData = GameManager.playData;

        character = Instantiate(catGirl);

        character.gameObject.SetActive(true);
        SetPos(pX, pY);

        reloadStack = uint.MaxValue;



        MaxExp = playData.maxExp;
        NowExp = playData.nowExp;
        BaseMaxHp = playData.baseMaxHp;
        NowHp = playData.nowHp;
        NowLevel = playData.level;

        BasePow = playData.basePow;
        BaseBalance = playData.baseBalance;
        BaseCriDamage = playData.baseCriDamage;
        BaseCriPer = playData.baseCriPer;

        MaxBullet = playData.maxBullet;
        NowBullet = playData.nowBullet;

        inventoryMgr.ClearItemData();

        //���� ����
        List<ItemObjData> weapons = playData.weaponItem;
        foreach (ItemObjData weaponObjData in weapons)
            if (inventoryMgr.AddItem(weaponObjData))
            {
                //�κ��丮�� ��� �ִ´�.
            }

        //�Ǽ��縮 ����
        List<ItemObjData> accessarys = playData.accessaryItem;
        foreach (ItemObjData accessaryObjData in accessarys)
            if (inventoryMgr.AddItem(accessaryObjData))
            {
                //�κ��丮�� ��� �ִ´�.
            }

        //��Ÿ ������ ����
        List<ItemObjData> etcs = playData.etcItem;
        foreach (ItemObjData etcObjData in etcs)
            if (inventoryMgr.AddItem(etcObjData))
            {
                //�κ��丮�� ��� �ִ´�.
            }

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp();
        totalUI.UpdateShield();
        totalUI.UpdateExp();
        totalUI.UpdatePlayerBullet();
        totalUI.ActKeyPad(true);
        canControl = true;

        StartCoroutine(runCatEvent());
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

        yield return new WaitUntil(() => (buttonInput != ButtonInput.None)); //��ư �Է��� ���� ���¸� ���

        yield return new WaitUntil(() => (characterMgr.canControl == true)); //��Ʈ�� �Ұ� �����ϰ�� ���

        int showDic = 0;
        Vector2Int dic = Vector2Int.zero;
        Vector3 bulletSpawnPos = character.GetBulletSpawnPos(buttonInput);
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
        if (characterMgr.nowBullet == 0)
        {
            //�Ѿ��� �ٴڳ���. �ܰ��÷��̸� ���ؼ�
            //�÷��׸� �����Ѵ�.
            useKnife = true;
        }

        int frontShowDic = character.animator.GetInteger("showDic");
        character.animator.SetInteger("showDic", showDic);

        uint weaponRange = nowWeapon.itemData.range;
        if (useKnife)
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

            jarObj = jarMgr.GetJarObj(cPos);
            if (jarObj != null)
            {
                //�׾Ƹ� �߰�
                if(i == 1)
                {
                    //�Ÿ��� 1�ۿ��ȵǸ� Į�� �μ���.
                    useKnife = true;
                }
                break;

            }

            if (i == 1)
            {
                chestObj = chestMgr.GetChestObj(cPos);
                if (chestObj != null)
                {
                    //���� �߰�
                    break;
                }
            }
        }

        character.animator.SetBool("useKnife", useKnife);

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
            //���ݵ����̴� 0
            reloadDelay = 0;
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
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("attack");
                    character.animator.SetBool("shotGun", nowShotGun);

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
                          dic.x == 0 ? bulletSpawnPos.x : targetMonster.pos.x * CreateMap.tileSize,
                          dic.y == 0 ? bulletSpawnPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

                    to += new Vector3(
                        dic.x == 0 ? Random.Range(-1, 1) : 0,
                        dic.y == 0 ? Random.Range(-1, 1) : 0) * 0.2f;

                    bulletMgr.FireBullet(bulletSpawnPos, to, duration);

                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("attack");
                    character.animator.SetBool("shotGun", nowShotGun);

                    if (nowShotGun)
                        CameraVibrate.Vibrate(10, 0.1f, 0.15f);
                    characterMgr.CostBullet();

                    yield return new WaitForSeconds(duration);

                    int totalDamage = characterMgr.GetTotalDamage();
                    bool critical = characterMgr.CriticalProcess(ref totalDamage);

                    targetMonster.Hit((uint)totalDamage, critical);

                    float remainTime = Mathf.Max(0, CatGirl.aniTime - duration);
                    yield return new WaitForSeconds(remainTime);
                }
            }
            else
            {
                if (useKnife)
                {
                    //���� �Ұ����̴�.
                    //�ܰ˻����̹Ƿ� ���
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("idle");
                    yield return new WaitForSeconds(CatGirl.aniTime);
                }
                else
                {
                    //���� �Ұ����̴�.
                    //���� �ִϸ��̼� ����
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("reload");
                    yield return new WaitForSeconds(CatGirl.aniTime);
                }
            }

            StartCoroutine(monsterMgr.RunMonster());

        }
        else if (chestObj != null)
        {
            chestObj.RemoveChestObj();

            yield return new WaitForSeconds(CatGirl.aniTime);
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
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("attack");
                    character.animator.SetBool("shotGun", nowShotGun);

                    yield return new WaitForSeconds(0.1f);

                    jarObj.RemoveJarObj();

                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    float gameDis = Vector2.Distance(pos, jarObj.pos);
                    float duration = 0.1f * gameDis;

                    Vector3 to = new Vector3(
                          dic.x == 0 ? bulletSpawnPos.x : jarObj.pos.x * CreateMap.tileSize
                        , dic.y == 0 ? bulletSpawnPos.y : jarObj.pos.y * CreateMap.tileSize, 0);

                    bulletMgr.FireBullet(bulletSpawnPos, to, duration);

                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("attack");
                    character.animator.SetBool("shotGun", nowShotGun);

                    if (nowShotGun)
                        CameraVibrate.Vibrate(10, 0.1f, 0.15f);
                    characterMgr.CostBullet();

                    yield return new WaitForSeconds(duration);

                    jarObj.RemoveJarObj();

                    float remainTime = Mathf.Max(0, CatGirl.aniTime - duration);
                    yield return new WaitForSeconds(remainTime);
                }
            }
            else
            {
                if (useKnife)
                {
                    //���� �Ұ����̴�.
                    //�ܰ˻����̹Ƿ� ���
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("idle");
                    yield return new WaitForSeconds(CatGirl.aniTime);
                }
                else
                {
                    //���� �Ұ����̴�.
                    //���� �ִϸ��̼� ����
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("reload");
                    yield return new WaitForSeconds(CatGirl.aniTime);
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
            character.CharacterFiip(spriteFiipX);
            StartCoroutine(monsterMgr.RunMonster());
            character.animator.SetTrigger("run");
            Vector3 to = character.transform.position + new Vector3(dic.x * CreateMap.tileSize, dic.y * CreateMap.tileSize, 0);
            yield return Action2D.MoveTo(character.transform, to, CatGirl.moveDuration);
            character.animator.SetTrigger("idle");
        }
        else
        {
            character.CharacterFiip(spriteFiipX);
            yield return monsterMgr.RunMonster();

            if (frontShowDic != showDic)
            {
                character.animator.SetTrigger("idle");
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
        if (itemObj != null)
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

        if (mapManager.GetEndPos() == pos)
        {
            //Ż�ⱸ �߰�
            //���� ������ �̵��Ѵ�.

            totalUI.ActKeyPad(false);
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX, pY�� �̵��� �� �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    private bool CanMove(Vector2Int pPos)
    {
        return CanMove(pPos.x, pPos.y);
    }
    private bool CanMove(int pX, int pY)
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
        if (jarMgr.IsJar(pX, pY))
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
        if (mapManager.CanMoveShop(pos, new Vector2Int(pX, pY)) == false)
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
    /// : �÷��̾��� ��ġ�� �������ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(int pX, int pY)
    {
        if (character == null)
            return;
        pos.x = pX;
        pos.y = pY;
        character.transform.position = new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);

        //�̴ϸ� ����
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //�ش� ��ġ�� ����� Ȱ��ȭ�Ѵ�.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        torchMgr.ActAreaTorch(pos.x, pos.y);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ĳ������ ������ǥ�� ��ġ
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int CharactorGamePos()
    {
        if (character == null)
        {
            return Vector2Int.zero;
        }
        return pos;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ĳ������ ���� ��ġ
    ////////////////////////////////////////////////////////////////////////////////
    public Vector3 CharactorTransPos()
    {
        if(character == null)
        {
            return Vector3.zero;
        }
        return character.transform.position;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾ pDamage��ŭ ���ظ� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void Hit(int pDamage)
    {
        if (use_Guardian_Ring)
        {
            //��ȣ�� ���� ����ߴ�.
            //�������� ����.
            gudrdian_cnt++;
            return;
        }

        TotalUI totalUI = TotalUI.instance;

        List<ItemObjData> nowAccessaryData = inventoryMgr.NowAccessaryList();
        ItemObjData nowWeaponData = inventoryMgr.NowWeapon();
        for (int idx = 0; idx < nowAccessaryData.Count; idx++)
        {
            if (nowAccessaryData[idx].item == Item.Guardian_Ring)
            {
                //���� �Ǽ��縮�� ��ȣ�� ���� ������ �ִ�.
                //�������� �޴� ��� ��ȣ�� ���� �ı��ȴ�.
                //�ı��� ������ó���� ��� ������ �Ͼ�Ƿ� �÷��׿� �ε������� ����������.
                use_Guardian_Ring = true;
                guardian_Ring_idx = idx;
                gudrdian_cnt++;
                return;
            }
        }

        int totalDamage = pDamage;
        totalDamage += ItemManager.GetTotalStatValue(nowAccessaryData, ItemStat.HitDamage);
        totalDamage += ItemManager.GetTotalStatValue(nowWeaponData, ItemStat.HitDamage);

        for (int cnt = 0; cnt < nowAccessaryData.Count; cnt++)
        {
            //���尡 ������� ���差�� ���� �����ۺ��� �Һ�ȴ�.
            int minIdx = -1;
            int minShield = int.MaxValue;

            for (int idx = 0; idx < nowAccessaryData.Count; idx++)
            {
                ItemObjData accessaryObjData = nowAccessaryData[idx];
                int shield = ItemManager.GetTotalStatValue(accessaryObjData, ItemStat.Shield);
                if (shield > 0 && minShield > shield)
                {
                    //���差�� ���� ���� �������� idx���� ���
                    minShield = shield;
                    minIdx = idx;
                }
            }

            if(minIdx == -1)
            {
                //���尡 ����.
                //���� ó�� ����
                break;
            }
            else
            {
                //���尡 �ִ�.
                //����� �������� ����Ų��.
                if(totalDamage > minShield)
                {
                    totalDamage -= minShield;
                    ItemManager.SetTotalStatValue(nowAccessaryData[minIdx], ItemStat.Shield, 0);
                }
                else
                {
                    minShield -= totalDamage;
                    totalDamage = 0;
                    ItemManager.SetTotalStatValue(nowAccessaryData[minIdx], ItemStat.Shield, minShield);

                    //�������� ����� ��� ������.
                    //���� ó�� ����
                    break;
                }
            }
        }


        //���� �������� ó���Ѵ�.
        if (nowHp > totalDamage)
            NowHp -= totalDamage;
        else
            NowHp = 0;

        totalUI.UpdateHp();
        totalUI.UpdateShield();
        StartCoroutine(HitStunDelay());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾� ������ �ִϸ��̼�
    ////////////////////////////////////////////////////////////////////////////////
    public void PlayHitAni()
    {
        if(gudrdian_cnt > 0)
        {
            gudrdian_cnt--;
            //��ȣ�� �� ������
            //������ �ִϸ��̼��� ������� �ʴ´�.
            return;
        }
        character.HitAni();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ��ȣ�Ǹ� ����Ʈó��
    /// : ����Ʈ ���� �� ��ȣ�� �� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void UseGuardianRing()
    {
        if (use_Guardian_Ring == false)
            return;
        use_Guardian_Ring = false;

        inventoryMgr.RemoveItem(ItemType.Accessary, guardian_Ring_idx);
        guardian_Ring_idx = -1;

        GurdianEffect.RunEffect();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ϵ�����
    ////////////////////////////////////////////////////////////////////////////////
    private IEnumerator HitStunDelay()
    {
        canControl = false;
        yield return new WaitForSeconds(0.5f);
        canControl = true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ġ ȹ��
    ////////////////////////////////////////////////////////////////////////////////
    public void GetExp(uint pValue)
    {
        float addExpRate = (100 + GetAddExpRate()) / 100f;
        pValue = (uint)(pValue * addExpRate);

        NowExp += pValue;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ġ ȹ��� ���
    ////////////////////////////////////////////////////////////////////////////////
    public int GetAddExpRate()
    {
        int rate = 0;
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();

        rate += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.AddExp);
        rate += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.AddExp);
        return rate;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �������� �������� �˻��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void LevelUpCheck()
    {
        if (nowExp >= maxExp)
        {
            //ȹ�����ġ�� �ִ����ġ�� �Ѱ�ٸ�
            //�����������Ѵ�.
            NowExp -= maxExp;

            LevelUp();
        }

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp(maxExp, nowExp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �÷��ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public void LevelUp()
    {
        //������ �÷��ش�.
        NowLevel += 1;

        //������ ����.
        TotalUI totalUI = TotalUI.instance;
        totalUI.ActSelectStatUI(true);

        //���� �ִ� ����ġ�� �������ش�.
        MaxExp = maxExp + 5 * ((NowLevel / 10) + 1);

    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Ѿ��� pValue��ŭ �Һ��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CostBullet(int pValue = 1)
    {
        TotalUI totalUI = TotalUI.instance;

        NowBullet -= pValue;
        if (NowBullet <= 0)
        {
            NowBullet = 0;
            if (nowBullet == 0)
            {
                totalUI.ShowExplainText(LanguageManager.GetText("AMMO_EMPTY"));
            }
        }

        if(nowBullet == 10)
        {
            totalUI.ShowExplainText(LanguageManager.GetText("AMMO_IS_LACK"));
        }

        totalUI.UpdatePlayerBullet();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Ѿ��� pValue��ŭ �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void GetBullet(int pValue = 1)
    {
        NowBullet += pValue;
        if (NowBullet > MaxBullet)
            NowBullet = MaxBullet;
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdatePlayerBullet();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �⺻ ���ݷ��� �ø���.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddPow(int pValue)
    {
        BasePow += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �⺻ �뷱���� �ø���.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddBalance(int pValue)
    {
        BaseBalance += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �⺻ ü���� �ø���.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMaxHp(int pValue)
    {
        BaseMaxHp += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ü���� ȸ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddNowHp(int pValue)
    {
        NowHp += pValue;
        NowHp = Mathf.Min(NowHp, GetTotalMaxHp());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ü���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalMaxHp()
    {
        int hp = BaseMaxHp;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        hp += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.Hp);
        hp += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.Hp);

        NowHp = Mathf.Min(nowHp, hp);

        return hp;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �������差�� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalShield()
    {
        int shield = 0;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        shield += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.Shield);
        shield += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.Shield);

        return shield;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalPow()
    {
        int pow = basePow;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        pow += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.Pow);
        pow += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.Pow);
        return pow;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����뷱���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalBalance()
    {
        int balance = baseBalance;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        balance += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.Balance);
        balance += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.Balance);
        balance = Mathf.Min(balance, MAX_BALANCE);
        return balance;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������������ �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalDamage()
    {
        //  minPow ~ Pow ���̷� �����ϰ� �������� �����ȴ�.
        int totalPow = GetTotalPow();

        int totalBalance = GetTotalBalance();
        float rate = (float)1 / (float)3;

        //��Ʈ�� �̿��� ������ �׷����� ���� ���� ����
        //x�� �ʹݿ��� per�� ���� ���� ũ����
        //x�� �Ĺݿ��� per�� ���� ���� ���� �۾�����.
        float lValue = Mathf.Pow(totalBalance, rate);
        float rValue = Mathf.Pow(MAX_BALANCE, rate);
        float per = lValue / rValue;
        int minPow = (int)(per * totalPow);

        int damage = Random.Range(minPow, totalPow);
        return damage;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ Ȯ���� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriPer()
    {
        float criPer = baseCriPer;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        criPer += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.CriRate);
        criPer += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.CriRate);
        criPer = Mathf.Min(criPer, 100);
        return criPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ �������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        float criDmg = baseCriDamage;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        criDmg += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.CriDmg);
        criDmg += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.CriDmg);
        return criDmg;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش� �������� ũ��Ƽ�÷� ������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool CriticalProcess(ref int pDamage)
    {
        float damage = pDamage;
        if (Random.Range(0,100) < GetTotalCriPer())
        {
            //ũ��Ƽ�÷� ��������.
            //true�� ��ȯ�ϰ�
            //ũ��Ƽ�ø�ŭ�� �������� �÷��ش�.
            damage += damage * (GetTotalCriDamage() / 100f);
            pDamage = (int)damage;
            return true;
        }
        return false;
    }
}
