using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerBulletUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nowBullet;
    [SerializeField]
    private TextMeshProUGUI notBullet;

    [SerializeField]
    private TextMeshProUGUI maxBullet;

    public void UpdateUI(int pNowBullet,int pMaxBullet)
    {
        if (pNowBullet == 0)
        {
            notBullet.enabled = true;
            nowBullet.enabled = false;
        }
        else
        {
            notBullet.enabled = false;
            nowBullet.enabled = true;
        }
        nowBullet.text = string.Format("{0:D3}", pNowBullet);
        maxBullet.text = string.Format("{0:D3}", pMaxBullet);
    }
}
