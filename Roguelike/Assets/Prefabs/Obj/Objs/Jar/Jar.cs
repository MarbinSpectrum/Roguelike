using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// :항아리객체
////////////////////////////////////////////////////////////////////////////////
public class Jar : Mgr
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
        body.SetActive(false);
        broken.SetActive(true);
        jarMgr.RemoveJarObj(pos);


        int r = Random.RandomRange(0, 100);
        if(r <= 3)
        {
            //3퍼센트의 확률로 탄창 드랍
            itemMgr.CreateItem(pos.x, pos.y, Item.Gun_Magazine);
        }
        else
        {
            //대부분 동전드랍
            itemMgr.CreateItem(pos.x, pos.y, Item.Coin);
        }

        monsterMgr.AwakeMonster(pos, 2);

    }
}
