using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : �������� ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////

public class ItemManager : DontDestroySingleton<ItemManager>
{
    [SerializeField]
    private List<ItemData> itemDataList = new List<ItemData>();
    private Dictionary<Vector2Int, ItemObj> items = new Dictionary<Vector2Int, ItemObj>();
    private Dictionary<Item, ItemObj> itemObjs;
    private Dictionary<Item, ItemData> itemDatas;

    public void Init()
    {
        if (itemObjs == null)
        {
            itemObjs = new Dictionary<Item, ItemObj>();
            itemDatas = new Dictionary<Item, ItemData>();
            foreach (ItemData itemData in itemDataList)
            {
                itemObjs[itemData.item] = itemData.itemObj;
                itemDatas[itemData.item] = itemData;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �ִ��� Ȯ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsItem(int pX,int pY)
    {
        ItemObj itemObj = GetItem(pX, pY);
        if (itemObj == null)
            return false;
        return true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �޾ƿ´�.
    ////////////////////////////////////////////////////////////////////////////////
    public ItemObj GetItem(int pX,int pY)
    {
        ItemObj itemObj = null;
        if (items.ContainsKey(new Vector2Int(pX, pY)))
            itemObj = items[new Vector2Int(pX, pY)];
        return itemObj;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateItem(int pX, int pY,Item pItem)
    {
        Init();

        if (IsItem(pX, pY))
            return;
        if (itemObjs.ContainsKey(pItem) == false)
            return;
        if (itemObjs[pItem] == null)
            return;
        ItemObj itemObj = Instantiate(itemObjs[pItem],transform);
        itemObj.itemObjData = CreateItemObjData(pItem);
        itemObj.pos = new Vector2Int(pX, pY);
        itemObj.transform.position =
            new Vector3(CreateMap.tileSize * pX, CreateMap.tileSize * pY, 0);
        itemObj.Init();

        items[new Vector2Int(pX, pY)] = itemObj;
    }

    public ItemObjData CreateItemObjData(Item pItem)
    {
        Init();
        ItemData itemData = GetItemData(pItem);
        if(itemData != null)
        {
            ItemObjData itemObjData = itemData.createItemObjData();
            return itemObjData;
        }
        return null;
    }

    public ItemData GetItemData(Item pItem)
    {
        Init();
         if (itemDatas.ContainsKey(pItem))
            return itemDatas[pItem];
        return null;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : �ʵ��� ��� �������� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveAll_Item()
    {
        Init();

        List<Vector2Int> posList = new List<Vector2Int>();

        foreach(KeyValuePair<Vector2Int, ItemObj> items in items)
        {
            posList.Add(items.Key);
        }

        foreach(Vector2Int pos in posList)
        {
            Destroy(items[pos].gameObject);
            items.Remove(new Vector2Int(pos.x, pos.y));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX pY ��ġ�� �������� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveItem(int pX, int pY)
    {
        Init();

        if (IsItem(pX, pY))
        {
            items.Remove(new Vector2Int(pX, pY));
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ItemStat�� �ؽ�Ʈ�� ���
    ////////////////////////////////////////////////////////////////////////////////
    public static string ItemStatNameStr(ItemStat itemStat)
    {
        switch (itemStat)
        {
            case ItemStat.Pow:
                return LanguageManager.GetText("ATK");
            case ItemStat.Balance:
                return LanguageManager.GetText("BALANCE");
            case ItemStat.CriRate:
                return LanguageManager.GetText("CRI_RATE");
            case ItemStat.CriDmg:
                return LanguageManager.GetText("CRI_DMG");
            case ItemStat.Hp:
                return LanguageManager.GetText("HP");
            case ItemStat.AddExp:
                return LanguageManager.GetText("ADD_EXP");
            case ItemStat.AddGold:
                return LanguageManager.GetText("ADD_GOLD");
        }
        return string.Empty;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ItemStat�� ���� �� ���
    ////////////////////////////////////////////////////////////////////////////////
    public static string ItemStatValueStr(ItemStat itemStat,int pValue)
    {
        switch (itemStat)
        {
            case ItemStat.Pow:
                return string.Format("{0}", pValue);
            case ItemStat.Balance:
                return string.Format("{0}", pValue);
            case ItemStat.CriRate:
                return string.Format("{0}%", pValue);
            case ItemStat.CriDmg:
                return string.Format("{0}%", pValue);
            case ItemStat.Hp:
                return string.Format("{0}", pValue);
            case ItemStat.AddExp:
                return string.Format("{0}%", pValue);
            case ItemStat.AddGold:
                return string.Format("{0}%", pValue);
        }
        return string.Empty;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData������� ������ �ɷ�ġ������ ��ȯ
    ////////////////////////////////////////////////////////////////////////////////
    public static string ItemStatDataStr(ItemObjData pItemObjData)
    {
        string str = "";
        List<ItemStatData> itemStatDatas = pItemObjData.itemStats;
        foreach (ItemStatData itemStatData in itemStatDatas)
        {
            ItemStat itemStat = itemStatData.itemStat;
            int statValue = itemStatData.dataValue;
            string nameText = ItemStatNameStr(itemStat);
            string valueText = ItemStatValueStr(itemStat, statValue);
            if (nameText == string.Empty || valueText == string.Empty)
            {
                //�̸��̳� ���� ������ ������� �ʴ´�.
                continue;
            }
            string statText = string.Format("{0} +{1}", nameText, valueText);

            bool isUpgrade = false;
            switch(itemStat)
            {
                case ItemStat.Pow:
                    {
                        int upgradeCnt = GetTotalStatValue(pItemObjData, ItemStat.CanUpgradePow);
                        if (upgradeCnt > 1)
                            isUpgrade = true;
                    }
                    break;
                case ItemStat.Balance:
                    {
                        int upgradeCnt = GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeBalance);
                        if (upgradeCnt > 1)
                            isUpgrade = true;
                    }
                    break;
                case ItemStat.CriDmg:
                    {
                        int upgradeCnt = GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeCriDmg);
                        if (upgradeCnt > 1)
                            isUpgrade = true;
                    }
                    break;
                case ItemStat.CriRate:
                    {
                        int upgradeCnt = GetTotalStatValue(pItemObjData, ItemStat.CanUpgradeCriRate);
                        if (upgradeCnt > 1)
                            isUpgrade = true;
                    }
                    break;
            }

            if(isUpgrade)
            {
                statText = "<color=#9999ff>" + statText + "</color>";
            }

            str += statText;
            str += "\n";
        }
        return str;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData������� ������ ������ ���
    ////////////////////////////////////////////////////////////////////////////////
    public static string ItemExplainDataStr(ItemObjData pItemObjData)
    {
        Item item = pItemObjData.itemData.item;
        int curse = GetTotalStatValue(pItemObjData, ItemStat.Curse);
        int hitDamage = GetTotalStatValue(pItemObjData, ItemStat.HitDamage);
        int hitDie = GetTotalStatValue(pItemObjData, ItemStat.HitDie);
        string str = LanguageManager.GetText(pItemObjData.itemData.explainKey);
        switch (item)
        {
            case Item.Wood_Ring:
                {
                    int shield = GetTotalStatValue(pItemObjData, ItemStat.Shield);
                    str = string.Format(str, shield);
                    return str;
                }

        }

        string str2 = "";

        if (hitDie > 0)
        {
            str2 += LanguageManager.GetText("HIT_DIE");
        }
        else if (hitDamage > 0)
        {
            str2 += LanguageManager.GetText("HIT_DAMAGE");
        }

        if (curse > 0)
        {
            str2 += LanguageManager.GetText("CANT_TAKE_OFF");
        }

        if (str2.Length > 0)
        {
            //������ ȿ���� ����ȿ�� �ؽ�Ʈ��
            //������ �ش�.
            str2 = "\n" + str2;
        }

        str += str2;

        return str;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemObjData�� ������ ���� ������ ���ȿ���� ����
    ////////////////////////////////////////////////////////////////////////////////
    public static void RunUseItemEffect(ItemObjData pItemObjData)
    {
        TotalUI totalUI = TotalUI.instance;

        List<ItemStatData> useEffect = pItemObjData.itemStats;
        foreach (ItemStatData itemStatData in useEffect)
        {
            ItemStat itemStat = itemStatData.itemStat;
            int value = itemStatData.dataValue;
            switch (itemStat)
            {
                case ItemStat.Heal:
                    {
                        //�÷��̾� ȸ��ȿ��
                        characterMgr.AddNowHp(value);
                    }
                    break;
            }
        }
        totalUI.UpdateHp();
        totalUI.UpdateShield();
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ItemType�� �޴´�.
    ////////////////////////////////////////////////////////////////////////////////
    public static ItemType GetItemType(ItemObj pItemObj)
    {
        ItemObjData itemObjData = pItemObj.itemObjData;
        return GetItemType(itemObjData);
    }
    public static ItemType GetItemType(ItemObjData pItemObjData)
    {
        ItemData itemData = pItemObjData.itemData;
        return GetItemType(itemData.item);
    }
    public static ItemType GetItemType(ItemData pItemData)
    {
        Item item = pItemData.item;
        return GetItemType(item);
    }
    public static ItemType GetItemType(Item pItem)
    {
        switch(pItem)
        {
            case Item.Glock17:
            case Item.M4:
            case Item.MP133:
            case Item.NormalGun:
                return ItemType.Weapon;
            case Item.Angry_Ring:
            case Item.Curse_Angry_Ring:

            case Item.Coolness_Ring:
            case Item.Curse_Coolness_Ring:

            case Item.Guardian_Ring:

            case Item.Life_Ring:
            case Item.Curse_Life_Ring:

            case Item.Gold_Ring:
            case Item.Silver_Ring:

            case Item.Leaf_Ring:
            case Item.Curse_Leaf_Ring:

            case Item.Skull_Ring:
            case Item.Curse_Skull_Ring:

            case Item.Wood_Ring:
                return ItemType.Accessary;
            case Item.Coin:
            case Item.ScrapMetal:
            default:
                return ItemType.Etc;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ְ� ������ ������ ������
    ////////////////////////////////////////////////////////////////////////////////
    public static Item UnCurseItem(Item pItem)
    {
        switch (pItem)
        {
            case Item.Curse_Leaf_Ring:
                return Item.Leaf_Ring;
            case Item.Curse_Life_Ring:
                return Item.Life_Ring;
            case Item.Curse_Angry_Ring:
                return Item.Angry_Ring;
            case Item.Curse_Coolness_Ring:
                return Item.Coolness_Ring;
            case Item.Curse_Skull_Ring:
                return Item.Skull_Ring;
        }

        return pItem;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �ִ� ������ �˻�
    ////////////////////////////////////////////////////////////////////////////////
    public static bool CantTakeOff(ItemObjData pItemObjData)
    {
        int value = GetTotalStatValue(pItemObjData, ItemStat.Curse);
        if (value > 0)
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������������ �˻�
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsUseItem(Item pItem)
    {
        switch (pItem)
        {
            case Item.Potion:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����°��� �˻�
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsDrink(Item pItem)
    {
        switch (pItem)
        {
            case Item.Potion:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �������� �˻�
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsShotGun(Item pItem)
    {
        switch (pItem)
        {
            case Item.MP133:
                return true;
            default:
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ��ȭ�� ���������� Ȯ���ϴ� �Լ�
    ////////////////////////////////////////////////////////////////////////////////
    public static bool IsUpgradeItem(ItemObjData pItemObjData)
    {
        int value = GetTotalStatValue(pItemObjData, ItemStat.IsUpgrade);
        if (value > 0)
        {
            //��ȭȽ���� 1�̻��̴�. ��ȭ������.
            return true;
        }
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����ۿ��� Ư�����ݰ��� ������ ��ȯ�ϴ� �Լ�
    ////////////////////////////////////////////////////////////////////////////////
    public static int GetTotalStatValue(List<ItemObjData> pItemList,ItemStat pItemStat)
    {
        if (pItemList == null)
            return 0;
        int sum = 0;
        foreach (ItemObjData itemObj in pItemList)
        {
            sum += GetTotalStatValue(itemObj, pItemStat);
        }
        return sum;
    }

    public static int GetTotalStatValue(ItemObjData pItemObjData, ItemStat pItemStat)
    {
        if (pItemObjData == null)
            return 0;
        int sum = 0;
        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //�ش� ���ݰ��� �����ش�.
                sum += itemStat.dataValue;
            }
        }
        return sum;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����ۿ��� Ư�����ݰ��� ��ȯ�ϴ� �Լ�
    ////////////////////////////////////////////////////////////////////////////////
    public static void SetTotalStatValue(ItemObjData pItemObjData, ItemStat pItemStat, int pValue)
    {
        if (pItemObjData == null)
            return;

        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //�ش� ���ݰ��� 0���� �ʱ�ȭ�Ѵ�.
                itemStat.dataValue = 0;
            }
        }

        foreach (ItemStatData itemStat in pItemObjData.itemStats)
        {
            if (itemStat.itemStat == pItemStat)
            {
                //���ݰ��� �����ش�.
                itemStat.dataValue = pValue;
                return;
            }
        }

        //�ش罺���� �׳� ���°� ����.
        //���� �߰����ش�.
        ItemStatData itemStatData = new ItemStatData(pItemStat, pValue, pValue);
        int dValue = itemStatData.dataValue;
        pItemObjData.itemStats.Add(itemStatData);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����ۿ��� Ư�����ݰ��� �߰��ϴ� �Լ�
    ////////////////////////////////////////////////////////////////////////////////
    public static void AddTotalStatValue(ItemObjData pItemObjData, ItemStat pItemStat, int pValue)
    {
        int StatValue = GetTotalStatValue(pItemObjData, pItemStat);
        SetTotalStatValue(pItemObjData, pItemStat, StatValue + pValue);
    }
}
