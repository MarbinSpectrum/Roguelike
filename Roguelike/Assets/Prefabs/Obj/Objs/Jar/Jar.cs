using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// :�׾Ƹ���ü
////////////////////////////////////////////////////////////////////////////////
public class Jar : MonoBehaviour
{
    public Vector2Int pos;
    public GameObject body;
    public GameObject broken;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �׾Ƹ� ��ü�� ����
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
