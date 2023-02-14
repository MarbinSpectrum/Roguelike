using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ĳ���͸� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class CharacterManager : FieldObjectSingleton<CharacterManager>
{
    [SerializeField]
    private CatGirl catGirl;

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
    private SoundObj changeWeaponSE;

    [HideInInspector]
    public CatGirl character;

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

    public const int MAX_ACCESSARY_SLOT = 4;

    [HideInInspector]
    public bool canControl;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY�� �ش��ϴ� ��ǥ�� ĳ���͸� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateCatGirl(int pX, int pY)
    {
        if(character != null)
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

        //�⺻���� �÷��̾�� ��� ��Ų��.
        ItemManager itemManager = ItemManager.instance;

        //���� ��� ����
        ItemObjData weaponObjData = itemManager.CreateItemObjData(startWeapon);
        if (weaponObjData != null)
        {
            //�ش� ��� �����Ѵ�.
            weaponObjData.equip = true;
            if (totalUI.ItemSendToInventory(weaponObjData))
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
            if (totalUI.ItemSendToInventory(accessaryObjData))
            {
                //�κ��丮�� ��� �ִ´�.
            }
        }

        canControl = true;
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
        TotalUI totalUI = TotalUI.instance;

        List<ItemObjData> nowAccessaryData = NowAccessaryList();
        
        for(int idx = 0; idx < nowAccessaryData.Count; idx++)
        {
            ItemData accessaryData = nowAccessaryData[idx].itemData;
            if (accessaryData.item == Item.Guardian_Ring)
            {
                //���� �Ǽ��縮�� ��ȣ�� ���� ������ �ִ�.
                //�������� �޴� ��� ��ȣ�� ���� �ı��ȴ�.
                totalUI.InventoryRemoveItem(ItemType.Accessary, idx);
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
        NowExp += pValue;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp();
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
    /// : �⺻ ü���� �ø���.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMaxHp(int pValue)
    {
        BaseMaxHp += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �⺻ �뷱���� �ø���.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddBalance(int pValue)
    {
        BaseBalance += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ü���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalMaxHp()
    {
        int hp = BaseMaxHp;
        ItemObjData nowWeaponData = NowWeapon();
        if (nowWeaponData != null)
        {
            List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.Hp)
                {
                    hp += itemStat.GetValue();
                }
            }
        }

        List<ItemObjData> nowAccessary = NowAccessaryList();
        foreach (ItemObjData accessary in nowAccessary)
        {
            foreach (ItemStatData itemStatData in accessary.itemStats)
            {
                if (itemStatData.itemStat == ItemStat.Hp)
                {
                    hp += itemStatData.GetValue();
                }
            }
        }

        NowHp = Mathf.Min(nowHp, hp);

        return hp;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalPow()
    {
        int pow = basePow;
        ItemObjData nowWeaponData = NowWeapon();
        List<ItemObjData> nowAccessary = NowAccessaryList();

        if (nowWeaponData != null)
        {
            List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.Pow)
                {
                    pow += itemStat.GetValue();
                }
            }
        }

        foreach (ItemObjData accessary in nowAccessary)
        {
            foreach (ItemStatData itemStat in accessary.itemStats)
            {
                if (itemStat.itemStat == ItemStat.Pow)
                {
                    pow += itemStat.GetValue();
                }
            }
        }

        return pow;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����뷱���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalBalance()
    {
        int balance = baseBalance;
        ItemObjData nowWeaponData = NowWeapon();
        List<ItemObjData> nowAccessary = NowAccessaryList();

        if (nowWeaponData != null)
        {
            List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.Balance)
                {
                    balance += itemStat.GetValue();
                }
            }
        }

        foreach (ItemObjData accessary in nowAccessary)
        {
            foreach (ItemStatData itemStat in accessary.itemStats)
            {
                if (itemStat.itemStat == ItemStat.Balance)
                {
                    balance += itemStat.GetValue();
                }
            }
        }
    
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

        ItemObjData nowWeaponData = NowWeapon();
        List<ItemObjData> nowAccessary = NowAccessaryList();

        if (nowWeaponData != null)
        {
            List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.CriPer)
                {
                    criPer += itemStat.GetValue();
                }
            }
        }

        foreach (ItemObjData accessary in nowAccessary)
        {
            foreach (ItemStatData itemStat in accessary.itemStats)
            {
                if (itemStat.itemStat == ItemStat.CriPer)
                {
                    criPer += itemStat.GetValue();
                }
            }
        }

        criPer = Mathf.Min(criPer, 100);
        return criPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ �������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        float criDmg = baseCriDamage;
        ItemObjData nowWeaponData = NowWeapon();
        List<ItemObjData> nowAccessary = NowAccessaryList();

        if (nowWeaponData != null)
        {
            List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.CriDmg)
                {
                    criDmg += itemStat.GetValue();
                }
            }
        }

        foreach (ItemObjData accessary in nowAccessary)
        {
            foreach (ItemStatData itemStat in accessary.itemStats)
            {
                if (itemStat.itemStat == ItemStat.CriDmg)
                {
                    criDmg += itemStat.GetValue();
                }
            }
        }

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

    public bool ChangeItem(ref ItemObjData pItemObjData)
    {
        if (pItemObjData == null)
        {
            //���� ��� ���°� ����.
            return false;
        }

        ItemObjData nowItemData = null;
        ItemType itemType = ItemManager.GetItemType(pItemObjData);
        if (itemType == ItemType.Weapon)
        {
            changeWeaponSE.PlaySE();
            nowItemData = NowWeapon();
        }
        else if (itemType == ItemType.Accessary)
        {
            //�Ǽ��縮�� �ٸ� ��� �ٷγ��� ������ �ƴϴ�.
            //������ ������ �صΰڴ�.
            List<ItemObjData> nowAccessary = NowAccessaryList();
            if(nowAccessary.Count > 0)
            {
                //0��° �������� ���Ƴ���.
                nowItemData = nowAccessary[0];
            }
        }
        else
        {
            //�ش� ���� ������ ��񰰴�.
            return false;
        }

        if (nowItemData != null)
        {
            bool cantTakeOff = ItemManager.CantTakeOff(nowItemData.itemData.item);
            if(cantTakeOff)
            {
                //������ ���� ����.
                return false;
            }

            //���� ��� ���´�..
            nowItemData.equip = false;
        }

        //�ش� ��� �����Ѵ�.
        pItemObjData.equip = true;

        return true;
    }

    public bool TakeOffItem(ref ItemObjData pItemObjData)
    {
        if(pItemObjData == null)
        {
            //���� ��� ���°� ����.
            return false;
        }

        pItemObjData.equip = false;

        return true;
    }

    public ItemObjData NowWeapon()
    {
        TotalUI totalUI = TotalUI.instance;
        return totalUI.GetNowWeaponToInventory();
    }

    public List<ItemObjData> NowAccessaryList()
    {
        TotalUI totalUI = TotalUI.instance;
        return totalUI.GetNowAccessaryToInventory();
    }

    public List<ItemObjData> HasAccessaryList()
    {
        TotalUI totalUI = TotalUI.instance;
        return totalUI.GetHasAccessaryToInventory();
    }
}
