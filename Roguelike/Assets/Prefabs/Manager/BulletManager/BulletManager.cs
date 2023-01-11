using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : FieldObjectSingleton<BulletManager>
{
    public Bullet bulletPrefab;
    public GameObject shotStartPrefab;
    public GameObject shotEndPrefab;

    private Queue<Bullet> bulletQueue = new Queue<Bullet>();
    private Queue<GameObject> shotStartQueue = new Queue<GameObject>();
    private Queue<GameObject> shotEndQueue = new Queue<GameObject>();

    private Bullet GetBullet()
    {
        Bullet bullet = null;

        if(bulletQueue.Count > 0)
        {
            bullet = bulletQueue.Dequeue();
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
        }

        return bullet;
    }

    public GameObject GetShotStart()
    {
        GameObject shotStart = null;

        if (shotStartQueue.Count > 0)
        {
            shotStart = shotStartQueue.Dequeue();
        }
        else
        {
            shotStart = Instantiate(shotStartPrefab);
        }
        shotStart.SetActive(false);

        return shotStart;
    }

    public GameObject GetShotEnd()
    {
        GameObject shotEnd = null;

        if (shotEndQueue.Count > 0)
        {
            shotEnd = shotEndQueue.Dequeue();
        }
        else
        {
            shotEnd = Instantiate(shotEndPrefab);
        }
        shotEnd.SetActive(false);

        return shotEnd;
    }

    public void FireBullet(Vector3 pFrom, Vector3 pTo,float pDuration)
    {
        Vector3 v = pTo - pFrom;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Bullet bullet = GetBullet();
        bullet.transform.eulerAngles = new Vector3(0, 0, angle);

        bullet.transform.position = pFrom;
        bullet.FireBullt(pTo, angle, pDuration);
    }

    public void DieBullet(Bullet pBullet,GameObject pShotStart, GameObject pShotEnd)
    {
        bulletQueue.Enqueue(pBullet);
        shotStartQueue.Enqueue(pShotStart);
        shotEndQueue.Enqueue(pShotEnd);
    }
}
