using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    public void PressLeftBtn()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.CharactorInputButton(ButtonInput.Left);
    }

    public void PressRightBtn()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.CharactorInputButton(ButtonInput.Right);
    }

    public void PressUpBtn()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.CharactorInputButton(ButtonInput.Up);
    }

    public void PressDownBtn()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.CharactorInputButton(ButtonInput.Down);
    }
}
