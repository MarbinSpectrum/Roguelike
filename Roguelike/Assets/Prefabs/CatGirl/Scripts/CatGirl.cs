using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl ĳ������ ��ũ��Ʈ
////////////////////////////////////////////////////////////////////////////////
public class CatGirl : Mgr
{
    private Animator Animator;
    public Animator animator { get => Animator ??= GetComponent<Animator>(); }
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer gunSprite;
    [SerializeField]
    private Transform effectBase;
    [SerializeField]
    private Transform gunBase;

    [SerializeField]
    private Material baseMaterial;
    [SerializeField]
    private Material hitMaterial;
    [SerializeField]
    private SoundObj hurtSE;

    [SerializeField]
    private Dictionary<ButtonInput, Transform> bulletPos = new Dictionary<ButtonInput, Transform>();

    public const float moveDuration = 0.3f;
    public const float aniTime = 0.3f;
    private IEnumerator hitCor;

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Է� ����Ű�� ���� �Ѿ� ��ȯ ��ġ
    ////////////////////////////////////////////////////////////////////////////////
    public Vector3 GetBulletSpawnPos(ButtonInput pButtonInput)
    {
        if (bulletPos.ContainsKey(pButtonInput))
            return bulletPos[pButtonInput].position;
        return Vector3.zero;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �޾������� �ִϸ��̼�
    ////////////////////////////////////////////////////////////////////////////////
    public void HitAni()
    {
        if (hitCor != null)
        {
            //���� ������ �ִϸ��̼� �ڷ�ƾ�� �������̸�
            //�ڷ�ƾ�� �ϴ� �����.
            StopCoroutine(hitCor);
            hitCor = null;
        }
        hurtSE.PlaySE();

        //������ �ִϸ��̼� �ڷ�ƾ ����
        hitCor = HitStunAni();
        StartCoroutine(hitCor);
    }
    protected virtual IEnumerator HitStunAni()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material = baseMaterial;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ĳ���� �¿� ������
    ////////////////////////////////////////////////////////////////////////////////
    public void CharacterFiip(bool pFilpX)
    {
        spriteRenderer.flipX = pFilpX;
        gunBase.localScale = new Vector3(pFilpX ? -1 : 1, 1, 1);
        effectBase.localScale = new Vector3(pFilpX ? -1 : 1, 1, 1);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �� ���� �̹����� �ٲ� (�ִϸ����Ϳ��� �����)
    ////////////////////////////////////////////////////////////////////////////////
    public void GunSpriteFront()
    {
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        Sprite sprite = nowWeapon.itemData.frontSprite;
        gunSprite.sprite = sprite;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �� ���� �̹����� �ٲ� (�ִϸ����Ϳ��� �����)
    ////////////////////////////////////////////////////////////////////////////////
    public void GunSpriteSide()
    {
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        Sprite sprite = nowWeapon.itemData.sideSprite;
        gunSprite.sprite = sprite;
    }
}
