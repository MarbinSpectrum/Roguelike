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

        int r = Random.RandomRange(0, 100);
        if(r <= 3)
        {
            //3퍼센트의 확률로 탄창 드랍
            itemManager.CreateItem(pos.x, pos.y, Item.Gun_Magazine);
        }
        else
        {
            //대부분 동전드랍
            itemManager.CreateItem(pos.x, pos.y, Item.Coin);
        }

    }
}
