using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �ѱ� ���ݿ� �÷����ִ� ������ ���� ������ �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchStatList : MonoBehaviour
{
    [SerializeField]
    private GunBenchStat gunBenchStatPrefabs;

    private List<GunBenchStat> gunStats = new List<GunBenchStat>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemStatDatas�� ����ִ� ������ ������ ǥ���մϴ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateUI(List<ItemStatData> pItemStatDatas)
    {
        foreach(GunBenchStat gunStat in gunStats)
        {
            //������� ���� ����Ʈ�� ��Ȱ��ȭ
            gunStat.gameObject.SetActive(false);
        }

        for(int i = 0; i < pItemStatDatas.Count; i++)
        {
            //���⼭���� ���ݸ���Ʈ�� ���� Ȱ��ȭ
            //ItemStat�� ���ؼ� ������ �̸��� �����ɴϴ�.
            //ItemStat�� �˸´� ������ statValue�� ǥ������.

            ItemStat itemStat = pItemStatDatas[i].itemStat;
            int statValue = pItemStatDatas[i].dataValue;

            GunBenchStat statObj = null;
            if (gunStats.Count <= i)
            {
                //���ݸ���Ʈ������ ��� ������ ǥ���� �� ���°����� �Ǵ�.
                //���ݸ���Ʈ�� ���ο� ��ü�� �߰��մϴ�.
                GunBenchStat newObj = Instantiate(gunBenchStatPrefabs);
                newObj.transform.parent = transform;
                newObj.transform.localScale = new Vector3(1, 1, 1);
                gunStats.Add(newObj);
            }

            //�ش� ������ ���Ȱ�ü�� �����ؼ�
            //������ ǥ���ϵ����Ѵ�.
            string statName = ItemManager.ItemStatNameStr(itemStat);
            string statValueStr = ItemManager.ItemStatValueStr(itemStat, statValue);
            if(statName.Length > 0)
            {
                statObj = gunStats[i];
                statObj.gameObject.SetActive(true);
                statObj.UpdateSlot(statName, statValueStr);
            }
        }

    }
}
