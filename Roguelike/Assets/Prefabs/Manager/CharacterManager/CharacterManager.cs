using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 캐릭터를 관리하는 매니저
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
    /// : pX,pY에 해당하는 좌표에 캐릭터를 생성한다.
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
        totalUI.UpdateHp(GetTotalMaxHp(), nowHp);
        totalUI.UpdateExp(maxExp, nowExp);

        //기본총을 플레이어에게 장비 시킨다.
        ItemManager itemManager = ItemManager.instance;

        //무기 장비 설정
        ItemObjData weaponObjData = itemManager.CreateItemObjData(startWeapon);
        if (weaponObjData != null)
        {
            //해당 장비를 장착한다.
            weaponObjData.equip = true;
            if (totalUI.ItemSendToInventory(weaponObjData))
            {
                //인벤토리에 장비를 넣는다.
            }
        }

        //악세사리 장비 설정
        ItemObjData accessaryObjData = itemManager.CreateItemObjData(startAccessary);
        if (accessaryObjData != null)
        {
            //해당 장비를 장착한다.
            accessaryObjData.equip = true;
            if (totalUI.ItemSendToInventory(accessaryObjData))
            {
                //인벤토리에 장비를 넣는다.
            }
        }

        canControl = true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 입력명령 버튼
    ////////////////////////////////////////////////////////////////////////////////
    public void CharactorInputButton(ButtonInput pButtonInput)
    {
        if (character == null)
            return;
        character.buttonInput = pButtonInput;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어가 pDamage만큼 피해를 받음
    ////////////////////////////////////////////////////////////////////////////////
    public void Hit(int pDamage)
    {
        TotalUI totalUI = TotalUI.instance;

        ItemObjData nowAccessaryData = NowAccessary();
        Item accessaryItem = Item.Null;

        if (nowAccessaryData != null)
        {
            //현재 아이템을 받아온다.
            accessaryItem = nowAccessaryData.itemData.item;
        }

        if (accessaryItem == Item.Guardian_Ring)
        {
            //현재 악세사리는 수호의 링이다.
            //데미지를 받는 대신 수호의 링이 파괴된다.
            totalUI.InventoryRemoveItem(ItemType.Accessary, 0);
        }
        else
        {
            //데미지를 받았다.
            if (nowHp > pDamage)
                NowHp -= pDamage;
            else
                NowHp = 0;

            totalUI.UpdateHp(GetTotalMaxHp(), nowHp);

            StartCoroutine(HitStunDelay());
        }     
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 스턴딜레이
    ////////////////////////////////////////////////////////////////////////////////
    private IEnumerator HitStunDelay()
    {
        canControl = false;
        yield return new WaitForSeconds(0.5f);
        canControl = true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 경험치 획득
    ////////////////////////////////////////////////////////////////////////////////
    public void GetExp(uint pValue)
    {
        NowExp += pValue;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp(maxExp, nowExp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 레벨업이 가능한지 검사한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void LevelUpCheck()
    {
        if (nowExp >= maxExp)
        {
            //획득경험치가 최대경험치를 넘겼다면
            //레벨업을을한다.
            NowExp -= maxExp;

            LevelUp();
        }

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp(maxExp, nowExp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 레벨을 올려준다.
    ////////////////////////////////////////////////////////////////////////////////
    public void LevelUp()
    {
        //레벨을 올려준다.
        NowLevel += 1;

        //스탯을 찍자.
        TotalUI totalUI = TotalUI.instance;
        totalUI.ActSelectStatUI(true);

        //다음 최대 경험치를 갱신해준다.
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
    /// : 기본 공격력을 올린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddPow(int pValue)
    {
        BasePow += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 기본 체력을 올린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMaxHp(int pValue)
    {
        BaseMaxHp += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 기본 밸런스를 올린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddBalance(int pValue)
    {
        BaseBalance += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종체력을 구해준다.
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

        ItemObjData nowAccessaryData = NowAccessary();
        if (nowAccessaryData != null)
        {
            List<ItemStatData> itemStatDatas = nowAccessaryData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.Hp)
                {
                    hp += itemStat.GetValue();
                }
            }
        }

        return hp;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 힘을 구해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalPow()
    {
        int pow = basePow;
        ItemObjData nowWeaponData = NowWeapon();
        if(nowWeaponData != null)
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

        ItemObjData nowAccessaryData = NowAccessary();
        if (nowAccessaryData != null)
        {
            List<ItemStatData> itemStatDatas = nowAccessaryData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
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
    /// : 최종밸런스를 구해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalBalance()
    {
        int balance = baseBalance;
        ItemObjData nowWeaponData = NowWeapon();
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

        ItemObjData nowAccessaryData = NowAccessary();
        if (nowAccessaryData != null)
        {
            List<ItemStatData> itemStatDatas = nowAccessaryData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
            {
                if (itemStat.itemStat == ItemStat.Balance)
                {
                    balance += itemStat.GetValue();
                }
            }
        }

        balance = Mathf.Min(balance, maxBalance);
        return balance;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종데미지를 구해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalDamage()
    {
        //  Pow*(balance/maxBalance) ~ Pow 사이로 랜덤하게 데미지가 결정된다.
        int totalPow = (int)GetTotalPow();
        float per = GetTotalBalance() / (float)maxBalance;
        int minPow = (int)(per * totalPow);

        int damage = Random.Range(minPow, totalPow);
        return damage;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 치명타 확률을 구한다.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriPer()
    {
        float criPer = baseCriPer;

        ItemObjData nowWeaponData = NowWeapon();
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

        ItemObjData nowAccessaryData = NowAccessary();
        if (nowAccessaryData != null)
        {
            List<ItemStatData> itemStatDatas = nowAccessaryData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
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
    /// : 최종 치명타 데미지를 구한다.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        float criDmg = baseCriDamage;
        ItemObjData nowWeaponData = NowWeapon();
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

        ItemObjData nowAccessaryData = NowAccessary();
        if (nowAccessaryData != null)
        {
            List<ItemStatData> itemStatDatas = nowAccessaryData.itemStats;
            foreach (ItemStatData itemStat in itemStatDatas)
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
    /// : 해당 데미지가 크리티컬로 적용될지 정한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool CriticalProcess(ref int pDamage)
    {
        float damage = pDamage;
        if (Random.Range(0,100) < GetTotalCriPer())
        {
            //크리티컬로 정해졌다.
            //true을 반환하고
            //크리티컬만큼의 데미지로 늘려준다.
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
            //현재 장비가 없는것 같다.
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
            nowItemData = NowAccessary();
        else
        {
            //해당 장비는 못끼는 장비같다.
            return false;
        }

        if (nowItemData != null)
        {
            //현재 장비를 벗는다..
            nowItemData.equip = false;
        }

        //해당 장비를 장착한다.
        pItemObjData.equip = true;

        return true;
    }

    public bool TakeOffItem(ref ItemObjData pItemObjData)
    {
        if(pItemObjData == null)
        {
            //현재 장비가 없는것 같다.
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

    public ItemObjData NowAccessary()
    {
        TotalUI totalUI = TotalUI.instance;
        return totalUI.GetNowAccessaryToInventory();
    }
}
