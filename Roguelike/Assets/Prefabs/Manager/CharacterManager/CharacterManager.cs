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
        else if (Input.GetKeyDown(KeyCode.Space))
            character.buttonInput = ButtonInput.Attack;

    }
}
