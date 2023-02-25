using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : �ѱ� ���ݿ� �÷����ִ� ������ ������ ǥ�����ݴϴ�.
////////////////////////////////////////////////////////////////////////////////
public class GunBenchStat : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statName;

    [SerializeField]
    private TextMeshProUGUI statNum;

    ////////////////////////////////////////////////////////////////////////////////
    /// ������ ���� ������ ���� ������ ǥ���մϴ�.
    /// : pStatNameStr : ������ �̸��� �ǹ��մϴ�.
    /// : pStatValueStr : ������ ���� �ǹ��մϴ�. 
    ///   (���ݸ��� ������ �ٸ��� ������ ǥ���ϴ� ����� �ٸ��ϴ�.)
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateSlot(string pStatNameStr,string pStatValueStr)
    {
        statName.text = pStatNameStr;
        statNum.text = pStatValueStr;
    }
}
