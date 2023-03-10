using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;



public class Bullet : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer bulletBody;
    [System.NonSerialized]
    public Item isBulletType;


    public void FireBullt(Item pIsBulletType, Vector3 pTo, float pAngle, float pDuration)
    {
        isBulletType = pIsBulletType;
        StartCoroutine(RunFireBullet(pTo, pAngle, pDuration));
    }

    private IEnumerator RunFireBullet(Vector3 pTo, float pAngle,float pDuration)
    {
        BulletManager bulletManager = BulletManager.instance;

        bulletBody.sprite = bulletManager.GetBulletSprite(isBulletType);

        BulletEffect shotStartEffect = bulletManager.GetShotStart();
        shotStartEffect.transform.position = transform.position;
        shotStartEffect.gameObject.SetActive(true);
        shotStartEffect.RunBulletAni(isBulletType);
        shotStartEffect.transform.eulerAngles = new Vector3(0, 0, pAngle);

        bulletBody.enabled = true;

        yield return Action2D.MoveTo(transform, pTo, pDuration);

        bulletBody.enabled = false;

        BulletEffect shotEndEffect = bulletManager.GetShotEnd();
        shotEndEffect.transform.position = transform.position;
        shotEndEffect.gameObject.SetActive(true);
        shotEndEffect.RunBulletAni(isBulletType);
        shotEndEffect.transform.eulerAngles = new Vector3(0, 0, pAngle);

        bulletManager.DieBullet(this, shotStartEffect, shotEndEffect);
    }
}
