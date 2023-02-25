using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

////////////////////////////////////////////////////////////////////////////////
/// : �� �̸� ǥ�ð����ؼ� ó���ϴ� ���Դϴ�.
////////////////////////////////////////////////////////////////////////////////
public class MapName : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI mapNameText;
    [SerializeField]
    private TextMeshProUGUI aniText;

    [SerializeField]
    private Animation showMapName;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �� �̸��� ǥ���մϴ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void ShowMapName(string pStr, bool pPlayAni)
    {
        mapNameText.text = pStr;
        aniText.text = pStr;
        if (pPlayAni)
        {
            //�� ���۽� ���� ���̸��� ǥ���մϴ�.
            showMapName.Play();
        }
    }
}
