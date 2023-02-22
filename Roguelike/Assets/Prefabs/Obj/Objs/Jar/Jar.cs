using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// :항아리객체
////////////////////////////////////////////////////////////////////////////////
public class Jar : MonoBehaviour
{
    public Vector2Int pos;
    public GameObject body;
    public GameObject broken;
    public int jarIdx;

    public void Init()
    {
        body.SetActive(true);
        broken.SetActive(false);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 항아리 객체를 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveJarObj()
    {
        JarManager jarManager = JarManager.instance;
        body.SetActive(false);
        broken.SetActive(true);
        jarManager.RemoveJarObj(pos);

        ItemManager itemManager = ItemManager.instance;
        itemManager.CreateItem(pos.x, pos.y, Item.Coin);

    }
}
