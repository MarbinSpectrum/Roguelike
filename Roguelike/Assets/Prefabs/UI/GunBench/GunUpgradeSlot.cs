using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : ���׷��̵� �����Դϴ�. ���׷��̵忡 ���� ������ �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
public class GunUpgradeSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI upgradeExplain;
    [SerializeField]
    private TextMeshProUGUI numText;
    [SerializeField]
    private TextMeshProUGUI numTextRed;

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ������ �������� ���׷��̵� ������ ǥ���մϴ�.
    /// : pCost : ���׷��̵� ����Դϴ�.
    /// : pHasCost : �÷��̾ ���� ������ �ִ� ��带 �ǹ��մϴ�.
    /// : pExplainStr : ���׷��̵忡 ���� ���� �ؽ�Ʈ�Դϴ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void UpdateUI(uint pCost,uint pHasCost,string pExplainStr)
    {
        upgradeExplain.text = pExplainStr;
        if(pCost <= pHasCost)
        {
            //����� ����մϴ�.
            //������� ���ڷ� ǥ���մϴ�.
            numText.enabled = true;
            numTextRed.enabled = false;
            numText.text = pCost.ToString();
        }
        else
        {
            //����� �����մϴ�.
            //������ ���ڷ� ǥ���մϴ�.
            numText.enabled = false;
            numTextRed.enabled = true;
            numTextRed.text = pCost.ToString();
        }
    }
}
