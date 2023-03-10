using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 총알을 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class BulletManager : FieldObjectSingleton<BulletManager>
{
    public Bullet bulletPrefab;
    public BulletEffect shotStartPrefab;
    public BulletEffect shotEndPrefab;

    [Space(40)]

    public Dictionary<Item, Sprite> itemBulletSprite = new Dictionary<Item, Sprite>();

    private Queue<Bullet> bulletQueue = new Queue<Bullet>();
    private Queue<BulletEffect> shotStartQueue = new Queue<BulletEffect>();
    private Queue<BulletEffect> shotEndQueue = new Queue<BulletEffect>();
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
            BulletEffect shotStart = Instantiate(shotStartPrefab);
            BulletEffect shotEnd = Instantiate(shotEndPrefab);

            bullet.transform.parent = transform;
            shotStart.transform.parent = transform;
            shotEnd.transform.parent = transform;

            bullet.gameObject.SetActive(false);
            shotStart.gameObject.SetActive(false);
            shotEnd.gameObject.SetActive(false);
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
    public BulletEffect GetShotStart()
    {
        BulletEffect shotStart = null;

        if (shotStartQueue.Count > 0)
        {
            shotStart = shotStartQueue.Dequeue();
        }
        else
        {
            shotStart = Instantiate(shotStartPrefab);
        }
        shotStart.gameObject.SetActive(false);
        shotStart.OffAni();

        return shotStart;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알 끝 객체를 받아온다.
    ////////////////////////////////////////////////////////////////////////////////
    public BulletEffect GetShotEnd()
    {
        BulletEffect shotEnd = null;

        if (shotEndQueue.Count > 0)
        {
            shotEnd = shotEndQueue.Dequeue();
        }
        else
        {
            shotEnd = Instantiate(shotEndPrefab);
        }
        shotEnd.gameObject.SetActive(false);
        shotEnd.OffAni();

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

        InventoryManager inventoryManager = InventoryManager.instance;
        ItemObjData nowWeapon = inventoryManager.NowWeapon();
        Item nowItem = nowWeapon.item;
        //현재 무기에 맞는 총알로 설정해준다.

        bullet.FireBullt(nowItem, pTo, angle, pDuration);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총알이 죽었다. 총알 객체들을 큐에 반환한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void DieBullet(Bullet pBullet, BulletEffect pShotStart, BulletEffect pShotEnd)
    {
        bulletQueue.Enqueue(pBullet);
        shotStartQueue.Enqueue(pShotStart);
        shotEndQueue.Enqueue(pShotEnd);
    }

    public Sprite GetBulletSprite(Item pItem)
    {
        Sprite sprite = null;
        if (itemBulletSprite.ContainsKey(pItem))
        {
            sprite = itemBulletSprite[pItem];
        }
        return sprite;
    }
}
