using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemStatData 
{
    public ItemStat itemStat;
    public Vector2Int valueMinMax;
    private bool hasValue;
    private int dataValue;
    public ItemStatData(ItemStat pItemStat,int pValueMin, int pValueMax)
    {
        itemStat = pItemStat;
        valueMinMax = new Vector2Int();
        hasValue = false;
        dataValue = 0;
        valueMinMax.Set(pValueMin, pValueMax);
    }

    public ItemStatData(ItemStatData pItemStatData)
    {
        itemStat = pItemStatData.itemStat;
        valueMinMax = new Vector2Int(pItemStatData.valueMinMax.x, pItemStatData.valueMinMax.y);
        hasValue = pItemStatData.hasValue;
        dataValue = pItemStatData.dataValue;
    }

    public int GetValue()
    {
        if(hasValue)
        {
            return dataValue;
        }
        hasValue = true;
        dataValue = Random.Range(valueMinMax.x, valueMinMax.y);
        return dataValue;
    }
}
