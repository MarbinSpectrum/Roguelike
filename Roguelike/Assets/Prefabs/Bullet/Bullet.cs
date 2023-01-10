using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
public class Bullet : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletBody;

    public void FireBullt(Vector2Int pTo, float pDuration)
    {
        StartCoroutine(RunFireBullet(pTo, pDuration));
    }

    private IEnumerator RunFireBullet(Vector2Int pTo, float pDuration)
    {
        bulletBody.SetActive(true);

        Vector3 to = new Vector3(pTo.x * CreateMap.tileSize, pTo.y * CreateMap.tileSize, 0);

        yield return Action2D.MoveTo(transform, to, pDuration);

        bulletBody.SetActive(false);

        BulletManager bulletManager = BulletManager.instance;
        bulletManager.DieBullet(this);
    }
}
