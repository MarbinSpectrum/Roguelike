using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// :�׾Ƹ���ü
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
    /// : �׾Ƹ� ��ü�� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveJarObj()
    {
        body.SetActive(false);
        broken.SetActive(true);
        jarMgr.RemoveJarObj(pos);


        int r = Random.RandomRange(0, 100);
        if(r <= 3)
        {
            //3�ۼ�Ʈ�� Ȯ���� źâ ���
            itemMgr.CreateItem(pos.x, pos.y, Item.Gun_Magazine);
        }
        else
        {
            //��κ� �������
            itemMgr.CreateItem(pos.x, pos.y, Item.Coin);
        }

        monsterMgr.AwakeMonster(pos, 2);

    }
}
