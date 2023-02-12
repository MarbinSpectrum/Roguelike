using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ī�޶� �������� ��ũ��Ʈ�Դϴ�.
////////////////////////////////////////////////////////////////////////////////
public class CameraVibrate : FieldObjectSingleton<CameraVibrate>
{
    private Vector3 basePos;
    private static IEnumerator corVibrate;

    private void Start()
    {
        basePos = transform.localPosition;
    }


    ////////////////////////////////////////////////////////////////////////////////
    /// : ī�޶� pVCnt Ƚ���� pPower�� ������ pDuration���� �Ͼ�ϴ�.
    ////////////////////////////////////////////////////////////////////////////////
    public static void Vibrate(int pVCnt, float pPower, float pDuration)
    {
        CameraVibrate cameraVibrate = instance;
        if (corVibrate != null)
        {
            cameraVibrate.StopCoroutine(corVibrate);
            corVibrate = null;
        }
        corVibrate = cameraVibrate.runVibrate(pVCnt, pPower, pDuration);
        cameraVibrate.StartCoroutine(corVibrate);
    }

    private IEnumerator runVibrate(int pVCnt, float pPower, float pDuration)
    {
        float t = pDuration / (float)pVCnt;
        for(int i = 0; i < pVCnt; i++)
        {
            float randomAngle = Random.Range(0, 360);
            Vector3 randomDic = Quaternion.Euler(0, 0, randomAngle) * Vector2.one* pPower;
            Vector3 randomPos = basePos + randomDic;
            transform.localPosition = randomPos;
            yield return new WaitForSeconds(t);
        }
        transform.localPosition = basePos;

    }
}
