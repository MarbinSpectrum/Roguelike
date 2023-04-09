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
        JarManager jarManager = JarManager.instance;
        body.SetActive(false);
        broken.SetActive(true);
        jarManager.RemoveJarObj(pos);

        ItemManager itemManager = ItemManager.instance;

        int r = Random.RandomRange(0, 100);
        if(r < 10)
        {
            //10�ۼ�Ʈ�� Ȯ���� źâ ���
            itemManager.CreateItem(pos.x, pos.y, Item.Gun_Magazine);
        }
        else
        {
            //��κ� �������
            itemManager.CreateItem(pos.x, pos.y, Item.Coin);
        }

    }
}
