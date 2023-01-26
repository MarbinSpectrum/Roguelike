using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CoinItem : ItemObj
{
    [MinMaxSlider(0, 100)]
    public Vector2Int coinMinMax;

    public override void OnEnable()
    {
        count = Random.Range(coinMinMax.x, coinMinMax.y);
    }

    public override void GetItem()
    {

    }
}
