using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ĳ���͸� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class CharacterManager : FieldObjectSingleton<CharacterManager>
{

    [SerializeField]
    private Item startWeapon;
    [SerializeField]
    private Item startAccessary;
    [SerializeField]
    private int startHp;
    [SerializeField]
    private int startPow;
    [SerializeField]
    private int startBalance;
    [SerializeField]
    private int startCriDamage;
    [SerializeField]
    private int startCriRate;

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
        }

        character = Instantiate(catGirl);

        character.gameObject.SetActive(true);
        character.SetPos(pX, pY);

        MaxExp = 10;
        NowExp = 0;
        BaseMaxHp = startHp;
        NowHp = GetTotalMaxHp();
        NowLevel = 1;

        BasePow = startPow;
        BaseBalance = startBalance;
        BaseCriDamage = startCriDamage;
        BaseCriPer = startCriRate;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp();
        totalUI.UpdateExp();

        ItemManager itemManager = ItemManager.instance;
        InventoryManager inventoryManager = InventoryManager.instance;

        //���� ��� ����
        ItemObjData weaponObjData = itemManager.CreateItemObjData(startWeapon);
        if (weaponObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            weaponObjData.equip = true;

            if (inventoryManager.AddItem(weaponObjData))
            {
                //�κ��丮�� ��� �ִ´�.
            }
        }

        //�Ǽ��縮 ��� ����
        ItemObjData accessaryObjData = itemManager.CreateItemObjData(startAccessary);
        if (accessaryObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            accessaryObjData.equip = true;
            if (inventoryManager.AddItem(accessaryObjData))
            {
                //�κ��丮�� ��� �ִ´�.
            }
        }

        canControl = true;
    }

    public Vector2Int CharactorGamePos()
    {
        if (character == null)
        {
            return Vector2Int.zero;
        }
        return character.GetPos();
    }
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

        InventoryManager inventoryManager = InventoryManager.instance;
        TotalUI totalUI = TotalUI.instance;

        List<ItemObjData> nowAccessaryData = inventoryManager.NowAccessaryList();

        for (int idx = 0; idx < nowAccessaryData.Count; idx++)
        {
            ItemData accessaryData = nowAccessaryData[idx].itemData;
            if (accessaryData.item == Item.Guardian_Ring)
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

        //�������� �޾Ҵ�.
        if (nowHp > pDamage)
            NowHp -= pDamage;
        else
            NowHp = 0;

        totalUI.UpdateHp();
        StartCoroutine(HitStunDelay());
    }

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

        InventoryManager inventoryManager = InventoryManager.instance;
        inventoryManager.RemoveItem(ItemType.Accessary, guardian_Ring_idx);
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
        InventoryManager inventoryManager = InventoryManager.instance;
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();
        ItemObjData nowWeapon = inventoryManager.NowWeapon();

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
        MaxExp = (uint)(maxExp * 1.5f);

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
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

        hp += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.Hp);
        hp += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.Hp);

        NowHp = Mathf.Min(nowHp, hp);

        return hp;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalPow()
    {
        int pow = basePow;
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

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
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

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
        //  Pow*(balance/maxBalance) ~ Pow ���̷� �����ϰ� �������� �����ȴ�.
        int totalPow = (int)GetTotalPow();
        float per = GetTotalBalance() / (float)MAX_BALANCE;
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

        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

        criPer += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.CriPer);
        criPer += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.CriPer);
        criPer = Mathf.Min(criPer, 100);
        return criPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ �������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        float criDmg = baseCriDamage;
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

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
