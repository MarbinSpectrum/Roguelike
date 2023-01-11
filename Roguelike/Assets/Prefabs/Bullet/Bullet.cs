using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;



public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletBody;

    public void FireBullt(Vector3 pTo, float pAngle, float pDuration)
    {
        StartCoroutine(RunFireBullet(pTo, pAngle, pDuration));
    }

    private IEnumerator RunFireBullet(Vector3 pTo, float pAngle,float pDuration)
    {
        BulletManager bulletManager = BulletManager.instance;

        GameObject shotStartEffect = bulletManager.GetShotStart();
        shotStartEffect.transform.position = transform.position;
        shotStartEffect.gameObject.SetActive(true);
        shotStartEffect.transform.eulerAngles = new Vector3(0, 0, pAngle);

        bulletBody.SetActive(true);

        yield return Action2D.MoveTo(transform, pTo, pDuration);

        bulletBody.SetActive(false);

        GameObject shotEndEffect = bulletManager.GetShotEnd();
        shotEndEffect.transform.position = transform.position;
        shotEndEffect.gameObject.SetActive(true);
        shotEndEffect.transform.eulerAngles = new Vector3(0, 0, pAngle);

        bulletManager.DieBullet(this, shotStartEffect, shotEndEffect);
    }
}
