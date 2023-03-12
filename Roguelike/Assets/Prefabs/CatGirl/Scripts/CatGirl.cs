using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl 캐릭터의 스크립트
////////////////////////////////////////////////////////////////////////////////
public class CatGirl : SerializedMonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer gunSprite;
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
    /// : 플레이어의 위치를 지정해준다.
    ////////////////////////////////////////////////////////////////////////////////
    public void SetPos(int pX, int pY)
    {
        pos.x = pX;
        pos.y = pY;
        transform.position = new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);

        //미니맵 갱신
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //해당 위치의 블록을 활성화한다.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        TorchManager.instance.ActAreaTorch(pos.x, pos.y);
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어의 위치를 받는다.
    ////////////////////////////////////////////////////////////////////////////////
    public Vector2Int GetPos()
    {
        return pos;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : pX, pY로 이동할 수 있는지 확인한다.
    ////////////////////////////////////////////////////////////////////////////////
    private bool CanMove(int pX,int pY)
    {
        MapManager mapManager = MapManager.instance;
        MonsterManager monsterManager = MonsterManager.instance;
        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;

        if (mapManager.IsWall(pX, pY))
        {
            return false;
        }
        if (monsterManager.IsMonster(pX, pY))
        {
            return false;
        }
        if(jarManager.IsJar(pX,pY))
        {
            return false;
        }
        if (chestManager.IsChest(pX, pY))
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

        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 캐릭터 조작에 따른 이벤트를 처리한다.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCatEvent()
    {
        MonsterManager monsterManager = MonsterManager.instance;
        MapManager mapManager = MapManager.instance;
        CharacterManager characterManager = CharacterManager.instance;
        ItemManager itemManager = ItemManager.instance;
        TotalUI totalUI = TotalUI.instance;
        TorchManager torchManager = TorchManager.instance;
        DamageEffect damageEffect = DamageEffect.instance;
        BulletManager bulletManager = BulletManager.instance;
        JarManager jarManager = JarManager.instance;
        ChestManager chestManager = ChestManager.instance;
        InventoryManager inventoryManager = InventoryManager.instance;

        ItemObjData nowWeapon = inventoryManager.NowWeapon();

        yield return new WaitUntil(()=> (buttonInput != ButtonInput.None)); //버튼 입력이 없는 상태면 대기

        yield return new WaitUntil(() => (characterManager.canControl == true)); //컨트롤 불가 상태일경우 대기

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

        int frontShowDic = animator.GetInteger("showDic");
        animator.SetInteger("showDic", showDic);

        uint weaponRange = nowWeapon.itemData.range;

        MonsterObj targetMonster = null;
        for (int i = 1; i <= weaponRange && targetMonster == null; i++)
        {
            //이동 방향에 몬스터가 존재하는지 검사한다.
            //공격범위안에 몬스터가 있으면 해당 객체를 가져온다.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
            {
                //도중에 벽을 만났다. 탐색중지
                break;
            }
            if (targetMonster == null)
            {
                //몬스터 발견
                targetMonster = monsterManager.IsMonster(cPos.x, cPos.y);
            }
        }

        Jar jarObj = null;
        for (int i = 1; i <= weaponRange && jarObj == null; i++)
        {
            //이동 방향에 항아리 객체가 존재하는지 검사한다.
            //공격범위안에 항아리가 있으면 해당 객체를 가져온다.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
            {
                //도중에 벽을 만났다. 탐색중지
                break;
            }
            if (jarObj == null)
            {
                //항아리 발견
                jarObj = jarManager.GetJarObj(cPos);
            }
        }

        Chest chestObj = null;
        {
            //이동 방향에 궤작 객체가 존재하는지 검사한다.
            Vector2Int cPos = new Vector2Int(pos.x + dic.x, pos.y + dic.y);
            if (chestObj == null)
            {
                //궤작 발견
                chestObj = chestManager.GetChestObj(cPos);
            }
        }

        //현재무기가 샷건인지 여부 확인
        bool nowShotGun = ItemManager.IsShotGun(nowWeapon.itemData.item);

        //공격 딜레이 확인
        //reloadStack이 reloadDelay를 넘어서면 공격가능하다.
        //reloadStack은 플레이어가 다른 조작을 할때마다 쌓인다.
        //해당 방식으로 공격의 딜레이를 넣었다.
        uint reloadDelay = nowWeapon.itemData.reloadDelay;
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
                //몬스터를 발견했고 발사가능한 상태

                reloadStack = 0; //장전스택을 0으로한다.

                float gameDis = Vector2.Distance(pos, targetMonster.pos);
                float duration = 0.075f * gameDis;

                Vector3 to = new Vector3(
                      dic.x == 0 ? bPos.x : targetMonster.pos.x * CreateMap.tileSize, 
                      dic.y == 0 ? bPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

                to += new Vector3(
                    dic.x == 0 ? Random.Range(-1, 1) : 0, 
                    dic.y == 0 ? Random.Range(-1, 1) : 0) * 0.2f;

                bulletManager.FireBullet(bPos, to, duration);

                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("attack");
                animator.SetBool("shotGun", nowShotGun);
                if(nowShotGun)
                    CameraVibrate.Vibrate(10, 0.1f, 0.15f);

                yield return new WaitForSeconds(duration);

                int totalDamage = characterManager.GetTotalDamage();
                bool critical = characterManager.CriticalProcess(ref totalDamage);

                targetMonster.Hit((uint)totalDamage, critical);

                float remainTime = Mathf.Max(0, aniTime - duration);
                yield return new WaitForSeconds(remainTime);
            }
            else
            {
                //공격 불가능이다.
                //장전 애니메이션 실행
                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("reload");
                yield return new WaitForSeconds(aniTime);
            }

            StartCoroutine(monsterManager.RunMonster());

        }
        else if(chestObj != null)
        {
            chestObj.RemoveChestObj();

            yield return new WaitForSeconds(aniTime);
        }
        else if (jarObj != null)
        {
            //이동 방향으로 항아리가 존재한다.
            //항아리를 부순다.

            if (canFire)
            {
                reloadStack = 0;

                float gameDis = Vector2.Distance(pos, jarObj.pos);
                float duration = 0.1f * gameDis;

                Vector3 to = new Vector3(
                      dic.x == 0 ? bPos.x : jarObj.pos.x * CreateMap.tileSize
                    , dic.y == 0 ? bPos.y : jarObj.pos.y * CreateMap.tileSize, 0);

                bulletManager.FireBullet(bPos, to, duration);

                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("attack");
                animator.SetBool("shotGun", nowShotGun);
                if (nowShotGun)
                    CameraVibrate.Vibrate(10, 0.1f, 0.15f);

                yield return new WaitForSeconds(duration);

                int totalDamage = characterManager.GetTotalDamage();

                //damageEffect.DamageEffectRun(jarObj.pos, totalDamage, false);

                jarObj.RemoveJarObj();

                float remainTime = Mathf.Max(0, aniTime - duration);
                yield return new WaitForSeconds(remainTime);
            }
            else
            {
                spriteRenderer.flipX = spriteFiipX;
                gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
                animator.SetTrigger("reload");
                yield return new WaitForSeconds(aniTime);
            }

            StartCoroutine(monsterManager.RunMonster());
        }
        else if (CanMove(pos.x + dic.x, pos.y + dic.y))
        {
            //해당 방향으로 이동가능한지 검사한다.
            //이동가능하면 이동한다.
            pos.x += dic.x;
            pos.y += dic.y;
            spriteRenderer.flipX = spriteFiipX;
            gunBase.localScale = new Vector3(spriteFiipX ? -1 : 1, 1, 1);
            StartCoroutine(monsterManager.RunMonster());
            animator.SetTrigger("run");
            Vector3 to = transform.position + new Vector3(dic.x * CreateMap.tileSize, dic.y * CreateMap.tileSize, 0);
            yield return Action2D.MoveTo(transform, to, moveDuration);
            animator.SetTrigger("idle");
        }
        else if(frontShowDic != showDic)
        {
            animator.SetTrigger("idle");
        }

        //해당 위치의 블록을 활성화한다.
        mapManager.ActAreaTile(pos.x, pos.y);

        //해당 위치의 토치를 활성화시킨다.
        torchManager.ActAreaTorch(pos.x, pos.y);

        //이동 위치에 아이템이 존재하는지 확인한다.
        ItemObj itemObj = itemManager.GetItem(pos.x, pos.y);
        if(itemObj != null)
        {
            //존재하면 아이템을 얻는다.
            itemObj.GetItem();
        }

        //미니맵 갱신
        totalUI.UpdateMiniMap(new Vector2Int(pos.x, pos.y), 4);

        //수호의 링을 사용했다면
        //이펙트와 함께 수호의 링을 없애준다.
        characterManager.UseGuardianRing();

        characterManager.LevelUpCheck(); //레벨업이 가능한지 검사.

        ButtonInput = ButtonInput.None;

        if(mapManager.GetEndPos() == pos)
        {
            //탈출구 발견
            //다음 씬으로 이동한다.
            yield return StageManager.LoadNextScene();
            yield break;
        }
        else if (mapManager.GetGunBenchPos() == pos)
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
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        Sprite sprite = nowWeapon.itemData.frontSprite;
        gunSprite.sprite = sprite;
    }

    public void GunSpriteSide()
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        Sprite sprite = nowWeapon.itemData.sideSprite;
        gunSprite.sprite = sprite;
    }
}
