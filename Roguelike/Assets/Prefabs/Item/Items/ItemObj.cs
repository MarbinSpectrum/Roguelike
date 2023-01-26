using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ItemObj : SerializedMonoBehaviour
{
    public Item item;
    public int count;
    public List<ItemStatData> itemStats = new List<ItemStatData>();

    public virtual void OnEnable()
    {

    }

    public virtual void GetItem()
    {

    }
}
