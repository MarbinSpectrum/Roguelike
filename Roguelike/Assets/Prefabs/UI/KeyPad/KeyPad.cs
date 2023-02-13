using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : UI Ű�е� ����
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
    /// : ���� ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressLeftBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Left;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressRightBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Right;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressUpBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Up;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Ʒ� ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressDownBtn()
    {
        if (actBtn == false)
            return;
        nowButton = ButtonInput.Down;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ��ư�� ������
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
