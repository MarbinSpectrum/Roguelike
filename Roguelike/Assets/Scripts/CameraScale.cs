using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraScale : SerializedMonoBehaviour
{
    //���� ȭ�� ����

    [SerializeField] private CameraFit cameraFit;
    [SerializeField] private RectTransform uiRect;

    [HideInInspector]
    public float Rate = 1;
    [ShowInInspector]
    public float rate
    {
        get { return Rate; }
        set
        {
            if(Rate  != value)
            {
                //���� ����Ǹ� UI�� ī�޶� ũ�⸦ ����

                Rate = value;
                if (cameraFit != null && uiRect != null)
                {
                    float v = Rate == 0 ? 0 : (1 / Rate); //ī�޶� ������

                    cameraFit.cameraRate = v;
                    uiRect.transform.localScale = new Vector2(v, v);
                }
            } 
        }
    }
}
