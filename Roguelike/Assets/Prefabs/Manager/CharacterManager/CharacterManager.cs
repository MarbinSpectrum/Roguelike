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
    }

    public void CharactorInputButton(ButtonInput pButtonInput)
    {
        character.buttonInput = pButtonInput;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            character.buttonInput = ButtonInput.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            character.buttonInput = ButtonInput.Right;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            character.buttonInput = ButtonInput.Up;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            character.buttonInput = ButtonInput.Down;
    }
}
