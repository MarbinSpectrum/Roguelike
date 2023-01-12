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

    public CatGirl character;

    private uint maxHp;
    private uint nowHp;

    private uint maxExp;
    private uint nowExp;

    private uint level;

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

        maxHp = 3;
        nowHp = 3;
        maxExp = 5;
        nowExp = 0;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);
        totalUI.UpdateExp(maxExp, nowExp);
    }

    public void CharactorInputButton(ButtonInput pButtonInput)
    {
        if (character == null)
            return;
        character.buttonInput = pButtonInput;
    }

    public void Hit(uint pDamage)
    {
        if (maxHp > pDamage)
            maxHp -= pDamage;
        else
            maxHp = 0;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);
    }

    public void GetExp(uint pValue)
    {
        nowExp += pValue;
        if(nowExp >= maxExp)
        {
            nowExp -= maxExp;
            maxExp = (uint)(maxExp *1.5f);
            level += 1;
        }
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp(maxExp, nowExp);
    }

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
}
