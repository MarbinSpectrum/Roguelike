using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl 캐릭터의 스크립트
////////////////////////////////////////////////////////////////////////////////
public class CatGirl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float moveSpeed = 0.5f;

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

        switch (buttonInput)
        {
            case ButtonInput.Left:
                {
                    spriteRenderer.flipX = true;
                    animator.SetInteger("showDic", 0);
                    if (CanMove(pos.x - 1, pos.y))
                    {
                        pos.x -= 1;

                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("run");
                        Vector3 to = transform.position + new Vector3(-3.55f, 0, 0);
                        yield return Action2D.MoveTo(transform, to, moveSpeed);
                        animator.SetTrigger("idle");
                    }
                    else
                    {
                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("idle");
                    }
                }
                break;
            case ButtonInput.Right:
                {
                    spriteRenderer.flipX = false;
                    animator.SetInteger("showDic", 0);
                    if (CanMove(pos.x + 1, pos.y))
                    {
                        pos.x += 1;

                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("run");
                        Vector3 to = transform.position + new Vector3(3.55f, 0, 0);
                        yield return Action2D.MoveTo(transform, to, moveSpeed);
                        animator.SetTrigger("idle");
                    }
                    else
                    {
                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("idle");
                    }
                }
                break;
            case ButtonInput.Up:
                {
                    animator.SetInteger("showDic", 1);
                    if (CanMove(pos.x, pos.y + 1))
                    {
                        pos.y += 1;

                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("run");
                        Vector3 to = transform.position + new Vector3(0, 3.55f, 0);
                        yield return Action2D.MoveTo(transform, to, moveSpeed);
                        animator.SetTrigger("idle");
                    }
                    else
                    {
                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("idle");
                    }
                }
                break;
            case ButtonInput.Down:
                {
                    animator.SetInteger("showDic", 2);
                    if (CanMove(pos.x, pos.y - 1))
                    {
                        pos.y -= 1;

                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("run");
                        Vector3 to = transform.position + new Vector3(0, -3.55f, 0);
                        yield return Action2D.MoveTo(transform, to, moveSpeed);
                        animator.SetTrigger("idle");
                    }
                    else
                    {
                        StartCoroutine(monsterManager.RunMonster());
                        animator.SetTrigger("idle");
                    }
                }
                break;
            case ButtonInput.Attack:
                {
                    animator.SetTrigger("attack");
 

                }
                break;
        }

        //해당 위치의 블록을 활성화한다.
        MapManager.instance.ActAreaTile(pos.x, pos.y);

        ButtonInput = ButtonInput.None;

        StartCoroutine(ButtonEventDelay());
    }
}
