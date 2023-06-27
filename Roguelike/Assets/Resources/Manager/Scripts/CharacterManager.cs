using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 캐릭터를 관리하는 매니저
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

    //버튼 입력처리
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
    /// : pX,pY에 해당하는 좌표에 캐릭터를 생성한다.
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

        //무기 설정
        List<ItemObjData> weapons = playData.weaponItem;
        foreach (ItemObjData weaponObjData in weapons)
            if (inventoryMgr.AddItem(weaponObjData))
            {
                //인벤토리에 장비를 넣는다.
            }

        //악세사리 설정
        List<ItemObjData> accessarys = playData.accessaryItem;
        foreach (ItemObjData accessaryObjData in accessarys)
            if (inventoryMgr.AddItem(accessaryObjData))
            {
                //인벤토리에 장비를 넣는다.
            }

        //기타 아이템 설정
        List<ItemObjData> etcs = playData.etcItem;
        foreach (ItemObjData etcObjData in etcs)
            if (inventoryMgr.AddItem(etcObjData))
            {
                //인벤토리에 장비를 넣는다.
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
    /// : 캐릭터 조작에 따른 이벤트를 처리한다.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCatEvent()
    {
        MapManager mapManager = MapManager.instance;
        TotalUI totalUI = TotalUI.instance;

        GetGoldEffect getGoldEffect = uIEffectMgr.getGoldEffect;
        DamageEffect damageEffect = uIEffectMgr.damageEffect;

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();

        yield return new WaitUntil(() => (buttonInput != ButtonInput.None)); //버튼 입력이 없는 상태면 대기

        yield return new WaitUntil(() => (characterMgr.canControl == true)); //컨트롤 불가 상태일경우 대기

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
            //총알이 바닥났다. 단검플레이를 위해서
            //플래그를 적용한다.
            useKnife = true;
        }

        int frontShowDic = character.animator.GetInteger("showDic");
        character.animator.SetInteger("showDic", showDic);

        uint weaponRange = nowWeapon.itemData.range;
        if (useKnife)
        {
            //총알이 바닥났다. 단검으로 플레이한다.
            //단검의 사거리는 1
            weaponRange = 1;
        }

        MonsterObj targetMonster = null;
        Jar jarObj = null;
        Chest chestObj = null;
        for (int i = 1; i <= weaponRange; i++)
        {
            //이동 방향에 오브젝트가 존재하는지 검사한다.
            //공격범위안에 오브젝트가 있으면 해당 객체를 가져온다.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
            {
                //도중에 벽을 만났다. 탐색중지
                break;
            }

            targetMonster = monsterMgr.IsMonster(cPos.x, cPos.y);
            if (targetMonster != null)
            {
                //몬스터 발견
                break;
            }

            jarObj = jarMgr.GetJarObj(cPos);
            if (jarObj != null)
            {
                //항아리 발견
                if(i == 1)
                {
                    //거리가 1밖에안되면 칼로 부순다.
                    useKnife = true;
                }
                break;

            }

            if (i == 1)
            {
                chestObj = chestMgr.GetChestObj(cPos);
                if (chestObj != null)
                {
                    //궤작 발견
                    break;
                }
            }
        }

        character.animator.SetBool("useKnife", useKnife);

        //현재무기가 샷건인지 여부 확인
        bool nowShotGun = ItemManager.IsShotGun(nowWeapon.itemData.item);

        //공격 딜레이 확인
        //reloadStack이 reloadDelay를 넘어서면 공격가능하다.
        //reloadStack은 플레이어가 다른 조작을 할때마다 쌓인다.
        //해당 방식으로 공격의 딜레이를 넣었다.
        uint reloadDelay = nowWeapon.itemData.reloadDelay;
        if (useKnife)
        {
            //총알이 바닥났다. 단검으로 플레이한다.
            //공격딜레이는 0
            reloadDelay = 0;
        }

        bool canFire = false;
        if (reloadDelay <= reloadStack)
        {
            //발사가능
            canFire = true;
        }
        else
        {
            //스택을 더채운다.
            reloadStack++;
        }

        if (targetMonster != null)
        {
            if (canFire)
            {
                //몬스터를 발견했고 공격이 가능한 상태

                reloadStack = 0; //장전스택(공격스택)을 0으로한다.

                if (useKnife)
                {
                    //단검으로 공격
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
                    //공격 불가능이다.
                    //단검상태이므로 대기
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("idle");
                    yield return new WaitForSeconds(CatGirl.aniTime);
                }
                else
                {
                    //공격 불가능이다.
                    //장전 애니메이션 실행
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
            //이동 방향으로 항아리가 존재한다.
            //항아리를 부순다.

            if (canFire)
            {
                reloadStack = 0;
                if (useKnife)
                {
                    //단검으로 공격
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
                    //공격 불가능이다.
                    //단검상태이므로 대기
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("idle");
                    yield return new WaitForSeconds(CatGirl.aniTime);
                }
                else
                {
                    //공격 불가능이다.
                    //장전 애니메이션 실행
                    character.CharacterFiip(spriteFiipX);
                    character.animator.SetTrigger("reload");
                    yield return new WaitForSeconds(CatGirl.aniTime);
                }
            }

            StartCoroutine(monsterMgr.RunMonster());
        }
        else if (CanMove(pos.x + dic.x, pos.y + dic.y))
        {
            //해당 방향으로 이동가능한지 검사한다.
            //이동가능하면 이동한다.
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

        //트랩 작동
        trapMgr.RunTrap();

        //해당 위치의 블록을 활성화한다.
        mapManager.ActAreaTile(pos.x, pos.y);

        //해당 위치의 토치를 활성화시킨다.
        torchMgr.ActAreaTorch(pos.x, pos.y);

        //이동 위치에 아이템이 존재하는지 확인한다.
        ItemObj itemObj = itemMgr.GetItem(pos.x, pos.y);
        if (itemObj != null)
        {
            //존재하면 아이템을 얻는다.
            itemObj.GetItem();
        }

        //미니맵 갱신
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //수호의 링을 사용했다면
        //이펙트와 함께 수호의 링을 없애준다.
        characterMgr.UseGuardianRing();

        characterMgr.LevelUpCheck(); //레벨업이 가능한지 검사.

        ButtonInput = ButtonInput.None;

        if (mapManager.GetEndPos() == pos)
        {
            //탈출구 발견
            //다음 씬으로 이동한다.

            totalUI.ActKeyPad(false);
            yield return StageManager.LoadNextMap();
            yield break;
        }
        else if (mapManager.GetGunBenchPos() == pos && GameManager.gunBenchAct)
        {
            //총기 작업대 발견
            //총기 작업대 UI실행
            totalUI.ActGunBench(true);
        }
        else if (mapManager.GetShopPos() == pos)
        {
            //상점 발견
            //상점 UI실행
            totalUI.ActShop(true);
        }

        StartCoroutine(runCatEvent());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX, pY로 이동할 수 있는지 확인한다.
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
    /// : 플레이어의 위치를 지정해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(int pX, int pY)
    {
        if (character == null)
            return;
        pos.x = pX;
        pos.y = pY;
        character.transform.position = new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);

        //미니맵 갱신
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //해당 위치의 블록을 활성화한다.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        torchMgr.ActAreaTorch(pos.x, pos.y);
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
        return pos;
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

        TotalUI totalUI = TotalUI.instance;

        List<ItemObjData> nowAccessaryData = inventoryMgr.NowAccessaryList();
        ItemObjData nowWeaponData = inventoryMgr.NowWeapon();
        for (int idx = 0; idx < nowAccessaryData.Count; idx++)
        {
            if (nowAccessaryData[idx].item == Item.Guardian_Ring)
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

        inventoryMgr.RemoveItem(ItemType.Accessary, guardian_Ring_idx);
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
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();

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
    /// : 총알을 pValue만큼 소비한다.
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
    /// : 총알을 pValue만큼 충전한다.
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

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

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

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

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

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

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

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

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
        //  minPow ~ Pow 사이로 랜덤하게 데미지가 결정된다.
        int totalPow = GetTotalPow();

        int totalBalance = GetTotalBalance();
        float rate = (float)1 / (float)3;

        //루트를 이용한 포물선 그래프를 토대로 값이 생성
        //x값 초반에는 per의 증가 폭이 크지만
        //x값 후반에는 per의 증가 폭이 점점 작아진다.
        float lValue = Mathf.Pow(totalBalance, rate);
        float rValue = Mathf.Pow(MAX_BALANCE, rate);
        float per = lValue / rValue;
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

        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        List<ItemObjData> nowAccessary = inventoryMgr.NowAccessaryList();

        criPer += ItemManager.GetTotalStatValue(nowWeapon, ItemStat.CriRate);
        criPer += ItemManager.GetTotalStatValue(nowAccessary, ItemStat.CriRate);
        criPer = Mathf.Min(criPer, 100);
        return criPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 최종 치명타 데미지를 구한다.
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
