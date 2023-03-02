using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 총기 선반에 올려져있는 무기의 여러 스텟을 관리합니다.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchStatList : MonoBehaviour
{
    [SerializeField]
    private GunBenchStat gunBenchStatPrefabs;

    private List<GunBenchStat> gunStats = new List<GunBenchStat>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : pItemStatDatas에 들어있는 스텟의 정보를 표시합니다.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateUI(List<ItemStatData> pItemStatDatas)
    {
        foreach(GunBenchStat gunStat in gunStats)
        {
            //사용중인 스텟 리스트를 비활성화
            gunStat.gameObject.SetActive(false);
        }

        for(int i = 0; i < pItemStatDatas.Count; i++)
        {
            //여기서부터 스텟리스트를 새로 활성화
            //ItemStat을 통해서 스텟의 이름을 가져옵니다.
            //ItemStat에 알맞는 단위로 statValue를 표시힙낟.

            ItemStat itemStat = pItemStatDatas[i].itemStat;
            int statValue = pItemStatDatas[i].dataValue;

            GunBenchStat statObj = null;
            if (gunStats.Count <= i)
            {
                //스텟리스트만으로 모든 스텟을 표시할 수 없는것으로 판단.
                //스텟리스트에 새로운 객체를 추가합니다.
                GunBenchStat newObj = Instantiate(gunBenchStatPrefabs);
                newObj.transform.parent = transform;
                newObj.transform.localScale = new Vector3(1, 1, 1);
                gunStats.Add(newObj);
            }

            //해당 정보를 스탯객체에 전달해서
            //스텟을 표시하도록한다.
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
