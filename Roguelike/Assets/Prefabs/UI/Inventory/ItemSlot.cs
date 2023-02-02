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
    [System.NonSerialized]
    public Item item = Item.Null;

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

    #region[private bool IsEquip]
    private bool IsEquip;
    public bool isEquip
    {
        get
        {
            return IsEquip;
        }
        set
        {
            IsEquip = value;
            UpdateSlot();
        }

    }
    #endregion

    [SerializeField]
    private GameObject equipView;

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
        if(item == Item.Coin)
            numText.text += "$";
        if (SlotSprite == null)
            slotImg.color = new Color(1, 1, 1, 0);
        else
            slotImg.color = new Color(1, 1, 1, 1);
        slotImg.sprite = SlotSprite;
        equipView.SetActive(IsEquip);
    }

    [SerializeField]
    private TextMeshProUGUI numText;
    [SerializeField]
    private Image slotImg;
}
