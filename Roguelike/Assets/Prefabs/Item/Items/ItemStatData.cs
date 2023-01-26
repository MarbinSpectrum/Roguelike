using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemStatData 
{
    public ItemStat itemStat;
    public float value;

    public ItemStatData(ItemStat pItemStat,float pValue)
    {
        itemStat = pItemStat;
        value = pValue;
    }
}
