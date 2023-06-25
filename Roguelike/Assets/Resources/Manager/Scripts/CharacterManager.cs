using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        character.SetPos(pX, pY);


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

        canControl = true;
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
        return character.GetPos();
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
    /// : �Է¸�� ��ư
    ////////////////////////////////////////////////////////////////////////////////
    public void CharactorInputButton(ButtonInput pButtonInput)
    {
        if (character == null)
            return;
        character.buttonInput = pButtonInput;
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
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (KeyPad.actBtn)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                character.buttonInput = ButtonInput.Left;
            else if (Input.GetKey(KeyCode.RightArrow))
                character.buttonInput = ButtonInput.Right;
            else if (Input.GetKey(KeyCode.UpArrow))
                character.buttonInput = ButtonInput.Up;
            else if (Input.GetKey(KeyCode.DownArrow))
                character.buttonInput = ButtonInput.Down;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            character.buttonInput = ButtonInput.None;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            character.buttonInput = ButtonInput.None;
        else if (Input.GetKeyUp(KeyCode.UpArrow))
            character.buttonInput = ButtonInput.None;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            character.buttonInput = ButtonInput.None;

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
