using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunSE : MonoBehaviour
{
    [SerializeField]
    private AudioClip normalGun;
    [SerializeField]
    private AudioClip shotGun;

    private void OnEnable()
    {
        PlaySE();
    }

    public void PlaySE()
    {
        SoundManager soundManager = SoundManager.instance;
        InventoryManager inventoryManager = InventoryManager.instance;

        if (soundManager == null || inventoryManager == null)
            return;

        ItemObjData nowWeapon = inventoryManager.NowWeapon();

        if (ItemManager.IsShotGun(nowWeapon.itemData.item))
        {
            soundManager.PlaySE(shotGun);
        }
        else
        {
            soundManager.PlaySE(normalGun);
        }
    }
}
