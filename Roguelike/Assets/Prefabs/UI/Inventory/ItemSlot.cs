using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : ������ ���� // �������̹����� ������ ǥ��
////////////////////////////////////////////////////////////////////////////////
public class ItemSlot : SerializedMonoBehaviour
{
    #region[private Sprite SlotSprite]
    private Sprite SlotSprite;
    public Sprite slotSprite
    {
        get
        {
            return SlotSprite;
        }
        set
        {
            SlotSprite = value;
            UpdateSlot();
        }

    }
    #endregion

    #region[private uint SlotNum]
    private uint SlotNum;
    public uint slotNum
    {
        get
        {
            return SlotNum;
        }
        set
        {
            SlotNum = value;
            UpdateSlot();
        }

    }
    #endregion

    private void Awake()
    {
        UpdateSlot();
    }

    private void UpdateSlot()
    {
        if (SlotNum <= 1)
            numText.enabled = false;
        else
            numText.enabled = true;
        numText.text = SlotNum.ToString();
        if(SlotSprite == null)
            slotImg.color = new Color(1, 1, 1, 0);
        else
            slotImg.color = new Color(1, 1, 1, 1);
        slotImg.sprite = SlotSprite;
    }

    [SerializeField]
    private TextMeshProUGUI numText;
    [SerializeField]
    private Image slotImg;
}