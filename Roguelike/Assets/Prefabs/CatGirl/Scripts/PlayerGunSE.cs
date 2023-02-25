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
        InventoryManager inventoryManager = InventoryManager.instance;

        if (inventoryManager == null)
            return;

        ItemObjData nowWeapon = inventoryManager.NowWeapon();

        if (ItemManager.IsShotGun(nowWeapon.itemData.item))
        {
            SoundManager.PlaySE(shotGun);
        }
        else
        {
            SoundManager.PlaySE(normalGun);
        }
    }
}
