using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 총알을 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class BulletManager : FieldObjectSingleton<BulletManager>
{
    public Bullet bulletPrefab;
    public GameObject shotStartPrefab;
    public GameObject shotEndPrefab;

    private Queue<Bullet> bulletQueue = new Queue<Bullet>();
    private Queue<GameObject> shotStartQueue = new Queue<GameObject>();
    private Queue<GameObject> shotEndQueue = new Queue<GameObject>();
    private bool isLoad = false;

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알 프리팹을 미리 오브젝트 큐에 넣는다.
    ////////////////////////////////////////////////////////////////////////////////
    public IEnumerator runCreateObj()
    {
        if (isLoad)
            yield break;
        isLoad = true;

        for (int i = 0; i < 3; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            GameObject shotStart = Instantiate(shotStartPrefab);
            GameObject shotEnd = Instantiate(shotEndPrefab);

            bullet.transform.parent = transform;
            shotStart.transform.parent = transform;
            shotEnd.transform.parent = transform;

            bullet.gameObject.SetActive(false);
            shotStart.SetActive(false);
            shotEnd.SetActive(false);
            DieBullet(bullet, shotStart, shotEnd);
            yield return new WaitForSeconds(0.1f);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알 객체를 받아온다.
    ////////////////////////////////////////////////////////////////////////////////
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
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알 발사 객체를 받아온다.
    ////////////////////////////////////////////////////////////////////////////////
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알 끝 객체를 받아온다.
    ////////////////////////////////////////////////////////////////////////////////
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : pFromd에서 pTo로 총알을 pDuration시간동안 발사한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void FireBullet(Vector3 pFrom, Vector3 pTo, float pDuration)
    {
        Vector3 v = pTo - pFrom;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Bullet bullet = GetBullet();
        bullet.transform.eulerAngles = new Vector3(0, 0, angle);

        bullet.transform.position = pFrom;
        bullet.FireBullt(pTo, angle, pDuration);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알이 죽었다. 총알 객체들을 큐에 반환한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void DieBullet(Bullet pBullet,GameObject pShotStart, GameObject pShotEnd)
    {
        bulletQueue.Enqueue(pBullet);
        shotStartQueue.Enqueue(pShotStart);
        shotEndQueue.Enqueue(pShotEnd);
    }
}
