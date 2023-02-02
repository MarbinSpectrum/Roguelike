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
    private uint BaseBalance;
    public uint baseBalance
    {
        get
        {
            return BaseBalance;
        }
    }
    #endregion

    private const uint maxBalance = 100;

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

        MaxExp = 5;
        NowExp = 0;
        MaxHp = 3;
        NowHp = MaxHp;
        NowLevel = 1;

        BasePow = 5;
        BaseBalance = 1;
        BaseCriDamage = 100;
        BaseCriPer = 5;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);
        totalUI.UpdateExp(maxExp, nowExp);

        //기본총을 플레이어에게 장비 시킨다.
        ItemManager itemManager = ItemManager.instance;
        ItemObjData itemObjData = itemManager.CreateItemObjData(Item.NormalGun);
        itemObjData.equip = true;
        if (totalUI.ItemSendToInventory(itemObjData))
        {

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
        if(nowExp >= maxExp)
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
        //레벨을 올려주고 공격력을 올려준다.
        NowLevel += 1;
        BasePow += 1;

        //다음 최대 경험치를 갱신해준다.
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
    /// : 최종 힘을 구해준다.
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
    /// : 최종밸런스를 구해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public uint GetTotalBalance()
    {
        return baseBalance;
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
        minPow = Mathf.Max(1, minPow);

        int damage = Random.Range(minPow, totalPow);
        return damage;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 치명타 확률을 구한다.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriPer()
    {
        return baseCriPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 치명타 데미지를 구한다.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        return baseCriDamage;
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

    public ItemObjData NowWeapon()
    {
        TotalUI totalUI = TotalUI.instance;
        return totalUI.GetNowWeaponToInventory();
    }
}
