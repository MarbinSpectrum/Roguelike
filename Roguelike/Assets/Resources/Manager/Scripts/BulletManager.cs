using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �Ѿ��� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class BulletManager : DontDestroySingleton<BulletManager>
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
    /// : �Ѿ� �������� �̸� ������Ʈ ť�� �ִ´�.
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
    /// : �Ѿ� ��ü�� �޾ƿ´�.
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
    /// : �Ѿ� �߻� ��ü�� �޾ƿ´�.
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
    /// : �Ѿ� �� ��ü�� �޾ƿ´�.
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
    /// : pFromd���� pTo�� �Ѿ��� pDuration�ð����� �߻��Ѵ�.
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
        //���� ���⿡ �´� �Ѿ˷� �������ش�.

        bullet.FireBullt(nowItem, pTo, angle, pDuration);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Ѿ��� �׾���. �Ѿ� ��ü���� ť�� ��ȯ�Ѵ�.
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
