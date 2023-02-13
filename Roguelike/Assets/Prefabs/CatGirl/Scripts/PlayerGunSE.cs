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
        CharacterManager characterManager = CharacterManager.instance;

        if (soundManager == null || characterManager == null)
            return;

        ItemObjData nowWeapon = characterManager.NowWeapon();

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
