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
    private float moveSpeed = 0.5f;
    [SerializeField]
    private int attackRange = 3;
    [SerializeField]
    private Dictionary<ButtonInput, Transform> bulletPos = new Dictionary<ButtonInput, Transform>();

    private ButtonInput ButtonInput = ButtonInput.None;

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
        StartCoroutine(ButtonEventDelay());
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
        for (int x = -3; x <= 3; x++)
            for (int y = -3; y <= 3; y++)
                totalUI.UpdateMiniMap(new Vector2Int(pos.x + x, pos.y + y));
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

        if (mapManager.IsWall(pX, pY))
        {
            return false;
        }
        if (monsterManager.IsMonster(pX, pY))
        {
            return false;
        }
        return true;
    }

    public IEnumerator ButtonEventDelay()
    {
        yield return new WaitUntil(()=> (buttonInput != ButtonInput.None));

        MonsterManager monsterManager = MonsterManager.instance;
        MapManager mapManager = MapManager.instance;

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

        animator.SetInteger("showDic", showDic); 
        MonsterObj targetMonster = null;
        for (int i = 1; i <= attackRange && targetMonster == null; i++)
        {
            Vector2Int cPos = new Vector2Int(pos.x + dic.x * i, pos.y + dic.y * i);
            if (mapManager.IsWall(cPos.x, cPos.y))
                break;
            if (targetMonster == null)
                targetMonster = monsterManager.IsMonster(cPos.x, cPos.y);
        }

        if (targetMonster != null)
        {
            float gameDis = Vector2.Distance(pos, targetMonster.pos);
            float duration = 0.1f * gameDis;

            Vector3 to = new Vector3(
                  dic.x == 0 ? bPos.x : targetMonster.pos.x * CreateMap.tileSize
                , dic.y == 0 ? bPos.y : targetMonster.pos.y * CreateMap.tileSize, 0);

            BulletManager bulletManager = BulletManager.instance;
            bulletManager.FireBullet(bPos, to, duration);

            spriteRenderer.flipX = spriteFiipX;
            animator.SetTrigger("attack");

            yield return new WaitForSeconds(duration);
            targetMonster.Hit(1);

            StartCoroutine(monsterManager.RunMonster());
        }
        else if (CanMove(pos.x + dic.x, pos.y + dic.y))
        {
            pos.x += dic.x;
            pos.y += dic.y;
            spriteRenderer.flipX = spriteFiipX;
            StartCoroutine(monsterManager.RunMonster());
            animator.SetTrigger("run");
            Vector3 to = transform.position + new Vector3(dic.x * CreateMap.tileSize, dic.y * CreateMap.tileSize, 0);
            yield return Action2D.MoveTo(transform, to, moveSpeed);
            animator.SetTrigger("idle");
        }
        else
        {
            animator.SetTrigger("idle");
        }

        //해당 위치의 블록을 활성화한다.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        //미니맵 갱신
        TotalUI totalUI = TotalUI.instance;
        for(int x = -3; x <= 3; x++)
            for(int y = -3; y <= 3; y++)
                totalUI.UpdateMiniMap(new Vector2Int(pos.x + x, pos.y + y));

        ButtonInput = ButtonInput.None;

        StartCoroutine(ButtonEventDelay());
    }
}
