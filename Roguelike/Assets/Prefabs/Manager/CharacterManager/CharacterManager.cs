using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 캐릭터를 관리하는 매니저
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
    /// : pX,pY에 해당하는 좌표에 캐릭터를 생성한다.
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

        ItemManager itemManager = ItemManager.instance;
        InventoryManager inventoryManager = InventoryManager.instance;

        //무기 장비 설정
        ItemObjData weaponObjData = itemManager.CreateItemObjData(startWeapon);
        if (weaponObjData != null)
        {
            //해당 장비를 장착한다.
            weaponObjData.equip = true;

            if (inventoryManager.AddItem(weaponObjData))
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
            if (inventoryManager.AddItem(accessaryObjData))
            {
                //인벤토리에 장비를 넣는다.
            }
        }

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp();
        totalUI.UpdateShield();
        totalUI.UpdateExp();


        canControl = true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 캐릭터의 게임좌표상 위치
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
    /// : 캐릭터의 실제 위치
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
        if (use_Guardian_Ring)
        {
            //수호의 링을 사용했다.
            //데미지가 없다.
            gudrdian_cnt++;
            return;
        }

        InventoryManager inventoryManager = InventoryManager.instance;
        TotalUI totalUI = TotalUI.instance;

        List<ItemObjData> nowAccessaryData = inventoryManager.NowAccessaryList();
        ItemObjData nowWeaponData = inventoryManager.NowWeapon();
        for (int idx = 0; idx < nowAccessaryData.Count; idx++)
        {
            ItemData accessaryData = nowAccessaryData[idx].itemData;
            if (accessaryData.item == Item.Guardian_Ring)
            {
                //현재 악세사리로 수호의 링을 가지고 있다.
                //데미지를 받는 대신 수호의 링이 파괴된다.
                //파괴는 데미지처리가 모드 끝난후 일어나므로 플레그와 인덱스값만 저장해주자.
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
            //쉴드가 있을경우 쉴드량이 작은 아이템부터 소비된다.
            int minIdx = -1;
            int minShield = int.MaxValue;

            for (int idx = 0; idx < nowAccessaryData.Count; idx++)
            {
                ItemObjData accessaryObjData = nowAccessaryData[idx];
                int shield = ItemManager.GetTotalStatValue(accessaryObjData, ItemStat.Shield);
                if (shield > 0 && minShield > shield)
                {
                    //쉴드량이 가장 작은 아이템의 idx값을 기록
                    minShield = shield;
                    minIdx = idx;
                }
            }

            if(minIdx == -1)
            {
                //쉴드가 없다.
                //쉴드 처리 종료
                break;
            }
            else
            {
                //쉴드가 있다.
                //쉴드로 데미지를 상쇄시킨다.
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

                    //데미지가 쉴드로 모두 막혔다.
                    //쉴드 처리 종료
                    break;
                }
            }
        }


        //남은 데미지를 처리한다.
        if (nowHp > totalDamage)
            NowHp -= totalDamage;
        else
            NowHp = 0;

        totalUI.UpdateHp();
        totalUI.UpdateShield();
        StartCoroutine(HitStunDelay());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 데미지 애니메이션
    ////////////////////////////////////////////////////////////////////////////////
    public void PlayHitAni()
    {
        if(gudrdian_cnt > 0)
        {
            gudrdian_cnt--;
            //수호의 링 사용상태
            //데미지 애니메이션이 실행되지 않는다.
            return;
        }
        character.HitAni();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 수호의링 이펙트처리
    /// : 이펙트 실행 및 수호의 링 제거
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
        float addExpRate = (100 + GetAddExpRate()) / 100f;
        pValue = (uint)(pValue * addExpRate);

        NowExp += pValue;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 경험치 획득률 계산
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
        MaxExp = maxExp + 5 * ((NowLevel / 10) + 1);

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
    /// : 기본 밸런스를 올린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddBalance(int pValue)
    {
        BaseBalance += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 기본 체력을 올린다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddMaxHp(int pValue)
    {
        BaseMaxHp += pValue;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 체력을 회복한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddNowHp(int pValue)
    {
        NowHp += pValue;
        NowHp = Mathf.Min(NowHp, GetTotalMaxHp());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종체력을 구해준다.
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
    /// : 최종쉴드량을 구해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalShield()
    {
        int shield = 0;
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

        shield += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.Shield);
        shield += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.Shield);

        return shield;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 힘을 구해준다.
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
    /// : 최종밸런스를 구해준다.
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
    /// : 최종데미지를 구해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalDamage()
    {
        //  Pow*(balance/maxBalance) ~ Pow 사이로 랜덤하게 데미지가 결정된다.
        int totalPow = (int)GetTotalPow();
        float per = GetTotalBalance() / (float)MAX_BALANCE;
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

        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryManager.NowAccessaryList();

        criPer += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.CriPer);
        criPer += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.CriPer);
        criPer = Mathf.Min(criPer, 100);
        return criPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 치명타 데미지를 구한다.
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
}
