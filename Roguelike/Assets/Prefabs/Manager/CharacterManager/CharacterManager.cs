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

    public CatGirl character;

    private uint maxHp;
    private uint nowHp;

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

        maxHp = 3;
        nowHp = 3;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);
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
