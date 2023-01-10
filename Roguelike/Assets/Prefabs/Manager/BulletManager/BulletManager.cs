using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : FieldObjectSingleton<BulletManager>
{
    public Bullet bulletPrefab;

    private Queue<Bullet> bulletQueue = new Queue<Bullet>();


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

    public void FireBullet(Vector2Int pFrom,Vector2Int pTo,float pDuration)
    {
        Bullet bullet = GetBullet();

        Vector3 from = new Vector3(pFrom.x * CreateMap.tileSize, pFrom.y * CreateMap.tileSize, 0);
        bullet.transform.position = from;

        bullet.FireBullt(pTo, pDuration);
    }

    public void DieBullet(Bullet pBullet)
    {
        bulletQueue.Enqueue(pBullet);
    }
}
