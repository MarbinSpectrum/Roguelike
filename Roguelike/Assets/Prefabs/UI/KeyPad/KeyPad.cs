using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : UI 키패드 조작
////////////////////////////////////////////////////////////////////////////////
public class KeyPad : MonoBehaviour
{
    private ButtonInput nowButton;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private static bool ActBtn = true;
    public static bool actBtn
    {
        get { return ActBtn; }
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : 왼쪽 버튼 눌렀을때
    ////////////////////////////////////////////////////////////////////////////////
    public void PressLeftBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Left;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 오른쪽 버튼 눌렀을때
    ////////////////////////////////////////////////////////////////////////////////
    public void PressRightBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Right;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 위쪽 버튼 눌렀을때
    ////////////////////////////////////////////////////////////////////////////////
    public void PressUpBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Up;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 아래 버튼 눌렀을때
    ////////////////////////////////////////////////////////////////////////////////
    public void PressDownBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Down;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 버튼을 땟을때
    ////////////////////////////////////////////////////////////////////////////////
    public void UpBtn()
    {
        nowButton = ButtonInput.None;
    }

    public void ActKeyPad(bool pState)
    {
        ActBtn = pState;
        foreach(Button btn in buttons)
        {
            btn.interactable = pState;
        }
        if(pState == false)
        {
            nowButton = ButtonInput.None; 
        }
    }

    private void Update()
    {
        CharacterManager characterManager = CharacterManager.instance;
        characterManager.CharactorInputButton(nowButton);
    }
}
