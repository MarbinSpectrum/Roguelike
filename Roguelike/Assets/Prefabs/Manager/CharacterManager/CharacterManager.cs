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

    [HideInInspector]
    public CatGirl character;

    #region[public uint maxHp]
    private uint MaxHp;
    public uint maxHp
    {
        get
        {
            return MaxHp;
        }
    }
    #endregion

    #region[public uint nowHp]
    private uint NowHp;
    public uint nowHp
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

    private const int maxBalance = 100;

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

        MaxExp = 5;
        NowExp = 0;
        MaxHp = 3;
        NowHp = MaxHp;
        NowLevel = 1;

        BasePow = 5;
        BaseBalance = 0;
        BaseCriDamage = 100;
        BaseCriPer = 5;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);
        totalUI.UpdateExp(maxExp, nowExp);

        //�⺻���� �÷��̾�� ��� ��Ų��.
        ItemManager itemManager = ItemManager.instance;
        ItemObjData itemObjData = itemManager.CreateItemObjData(startWeapon);
        itemObjData.equip = true;
        if (totalUI.ItemSendToInventory(itemObjData))
        {

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
    public void Hit(uint pDamage)
    {
        if (nowHp > pDamage)
            NowHp -= pDamage;
        else
            NowHp = 0;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);

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
        if(nowExp >= maxExp)
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
        //������ �÷��ְ� ���ݷ��� �÷��ش�.
        NowLevel += 1;
        BasePow += 1;

        //���� �ִ� ����ġ�� �������ش�.
        MaxExp = (uint)(maxExp * 1.5f);

    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            character.buttonInput = ButtonInput.Left;
        else if (Input.GetKey(KeyCode.RightArrow))
            character.buttonInput = ButtonInput.Right;
        else if (Input.GetKey(KeyCode.UpArrow))
            character.buttonInput = ButtonInput.Up;
        else if (Input.GetKey(KeyCode.DownArrow))
            character.buttonInput = ButtonInput.Down;

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
    /// : ���� ���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalPow()
    {
        int pow = basePow;
        ItemObjData nowWeaponData = NowWeapon();
        List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
        foreach(ItemStatData itemStat in itemStatDatas)
        {
            if(itemStat.itemStat == ItemStat.Pow)
            {
                pow += itemStat.GetValue();
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
        List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
        foreach (ItemStatData itemStat in itemStatDatas)
        {
            if (itemStat.itemStat == ItemStat.Balance)
            {
                balance += itemStat.GetValue();
            }
        }
        balance = Mathf.Min(balance, maxBalance);
        return balance;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������������ �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalDamage()
    {
        //  Pow*(balance/maxBalance) ~ Pow ���̷� �����ϰ� �������� �����ȴ�.
        int totalPow = (int)GetTotalPow();
        float per = GetTotalBalance() / (float)maxBalance;
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
        List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
        foreach (ItemStatData itemStat in itemStatDatas)
        {
            if (itemStat.itemStat == ItemStat.CriPer)
            {
                criPer += itemStat.GetValue();
            }
        }

        return criPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ �������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        float criDmg = baseCriDamage;
        ItemObjData nowWeaponData = NowWeapon();
        List<ItemStatData> itemStatDatas = nowWeaponData.itemStats;
        foreach (ItemStatData itemStat in itemStatDatas)
        {
            if (itemStat.itemStat == ItemStat.CriDmg)
            {
                criDmg += itemStat.GetValue();
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

    public ItemObjData NowWeapon()
    {
        TotalUI totalUI = TotalUI.instance;
        return totalUI.GetNowWeaponToInventory();
    }
}
