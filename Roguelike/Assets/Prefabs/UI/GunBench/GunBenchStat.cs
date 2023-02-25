using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : 총기 선반에 올려져있는 무기의 스텟을 표시해줍니다.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchStat : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statName;

    [SerializeField]
    private TextMeshProUGUI statNum;

    ////////////////////////////////////////////////////////////////////////////////
    /// 다음과 같은 정보를 토대로 스텟을 표시합니다.
    /// : pStatNameStr : 스텟의 이름을 의미합니다.
    /// : pStatValueStr : 스텟의 값을 의미합니다. 
    ///   (스텟마다 단위가 다르기 때문에 표기하는 방법도 다릅니다.)
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateSlot(string pStatNameStr,string pStatValueStr)
    {
        statName.text = pStatNameStr;
        statNum.text = pStatValueStr;
    }
}
