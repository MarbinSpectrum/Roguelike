using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraScale : SerializedMonoBehaviour
{
    //게임 화면 비율

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
                //값이 변경되면 UI와 카메라 크기를 수정

                Rate = value;
                if (cameraFit != null && uiRect != null)
                {
                    float v = Rate == 0 ? 0 : (1 / Rate); //카메라 비율값

                    cameraFit.cameraRate = v;
                    uiRect.transform.localScale = new Vector2(v, v);
                }
            } 
        }
    }
}
