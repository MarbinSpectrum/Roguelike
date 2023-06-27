using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : CatGirl 캐릭터의 스크립트
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
    /// : 입력 방향키에 따른 총알 소환 위치
    ////////////////////////////////////////////////////////////////////////////////
    public Vector3 GetBulletSpawnPos(ButtonInput pButtonInput)
    {
        if (bulletPos.ContainsKey(pButtonInput))
            return bulletPos[pButtonInput].position;
        return Vector3.zero;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 데미지 받았을때의 애니메이션
    ////////////////////////////////////////////////////////////////////////////////
    public void HitAni()
    {
        if (hitCor != null)
        {
            //만약 데미지 애니메이션 코루틴이 진행중이면
            //코루틴을 일단 멈춘다.
            StopCoroutine(hitCor);
            hitCor = null;
        }
        hurtSE.PlaySE();

        //데미지 애니메이션 코루틴 실행
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
    /// : 캐릭터 좌우 뒤집기
    ////////////////////////////////////////////////////////////////////////////////
    public void CharacterFiip(bool pFilpX)
    {
        spriteRenderer.flipX = pFilpX;
        gunBase.localScale = new Vector3(pFilpX ? -1 : 1, 1, 1);
        effectBase.localScale = new Vector3(pFilpX ? -1 : 1, 1, 1);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총 정면 이미지로 바꿈 (애니메이터에서 사용함)
    ////////////////////////////////////////////////////////////////////////////////
    public void GunSpriteFront()
    {
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        Sprite sprite = nowWeapon.itemData.frontSprite;
        gunSprite.sprite = sprite;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 총 옆면 이미지로 바꿈 (애니메이터에서 사용함)
    ////////////////////////////////////////////////////////////////////////////////
    public void GunSpriteSide()
    {
        ItemObjData nowWeapon = inventoryMgr.NowWeapon();
        Sprite sprite = nowWeapon.itemData.sideSprite;
        gunSprite.sprite = sprite;
    }
}
