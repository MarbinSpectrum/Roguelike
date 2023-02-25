using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class ItemStatData 
{
    public ItemStat itemStat;
    public Vector2Int valueMinMax;
    [HideInInspector]
    public int dataValue = -1;

    public ItemStatData(ItemStat pItemStat,int pValueMin, int pValueMax)
    {
        itemStat = pItemStat;
        valueMinMax = new Vector2Int();
        valueMinMax.Set(pValueMin, pValueMax);
        MakeDataValue();
    }

    public ItemStatData(ItemStatData pItemStatData)
    {
        itemStat = pItemStatData.itemStat;
        valueMinMax = new Vector2Int(pItemStatData.valueMinMax.x, pItemStatData.valueMinMax.y);
        dataValue = pItemStatData.dataValue;
        if(dataValue == -1)
            MakeDataValue();
    }

    public void MakeDataValue()
    {
        dataValue = UnityEngine.Random.Range(valueMinMax.x, valueMinMax.y);
    }
}
