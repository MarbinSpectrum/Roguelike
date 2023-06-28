using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : UI Ű�е� ����
////////////////////////////////////////////////////////////////////////////////
public class KeyPad : Mgr
{
    public static ButtonInput keyPadPressButton
    {
        get;
        private set;
    }

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    private static bool ActBtn = true;
    public static bool actBtn
    {
        get { return ActBtn; }
    }
    private IEnumerator InputUpdateCor;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Է¸�� ��ư
    ////////////////////////////////////////////////////////////////////////////////
    public void CharactorInputButton(ButtonInput pButtonInput)
    {
        characterMgr.buttonInput = pButtonInput;

        if (InputUpdateCor != null)
        {
            StopCoroutine(InputUpdateCor);
            InputUpdateCor = null;
        }
        keyPadPressButton = pButtonInput;
        if (keyPadPressButton == ButtonInput.None)
            return;
        InputUpdateCor = InputUpdate();
        StartCoroutine(InputUpdateCor);
    }


    private IEnumerator InputUpdate()
    {
        characterMgr.buttonInput = keyPadPressButton;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(InputUpdate());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressLeftBtn()
    {
        if (actBtn == false)
            return;
        keyPadPressButton = ButtonInput.Left;
        CharactorInputButton(keyPadPressButton);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressRightBtn()
    {
        if (actBtn == false)
            return;
        keyPadPressButton = ButtonInput.Right;
        CharactorInputButton(keyPadPressButton);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressUpBtn()
    {
        if (actBtn == false)
            return;
        keyPadPressButton = ButtonInput.Up;
        CharactorInputButton(keyPadPressButton);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Ʒ� ��ư ��������
    ////////////////////////////////////////////////////////////////////////////////
    public void PressDownBtn()
    {
        if (actBtn == false)
            return;
        keyPadPressButton = ButtonInput.Down;
        CharactorInputButton(keyPadPressButton);

    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ��ư�� ������
    ////////////////////////////////////////////////////////////////////////////////
    public void UpBtn()
    {
        keyPadPressButton = ButtonInput.None;
        CharactorInputButton(keyPadPressButton);
    }

    public void ActKeyPad(bool pState)
    {
        ActBtn = pState;
        foreach (Button btn in buttons)
        {
            btn.interactable = pState;
        }
        if (pState == false)
        {
            keyPadPressButton = ButtonInput.None;
            CharactorInputButton(keyPadPressButton);
        }
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (actBtn)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && keyPadPressButton != ButtonInput.Left)
            {
                keyPadPressButton = ButtonInput.Left;
                CharactorInputButton(ButtonInput.Left);
            }
            else if (Input.GetKey(KeyCode.RightArrow) && keyPadPressButton != ButtonInput.Right)
            {
                keyPadPressButton = ButtonInput.Right;
                CharactorInputButton(ButtonInput.Right);
            }
            else if (Input.GetKey(KeyCode.UpArrow) && keyPadPressButton != ButtonInput.Up)
            {
                keyPadPressButton = ButtonInput.Up;
                CharactorInputButton(ButtonInput.Up);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && keyPadPressButton != ButtonInput.Down)
            {
                keyPadPressButton = ButtonInput.Down;
                CharactorInputButton(ButtonInput.Down);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            CharactorInputButton(ButtonInput.None);
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            CharactorInputButton(ButtonInput.None);
        else if (Input.GetKeyUp(KeyCode.UpArrow))
            CharactorInputButton(ButtonInput.None);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            CharactorInputButton(ButtonInput.None);

    }
}
