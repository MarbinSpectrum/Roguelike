using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////////////////////////////////
/// : 인벤토리의 슬롯이미지를 갱신해준다.
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class UpdateSlotImg : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> slotSprite = new List<Sprite>();
    [SerializeField]
    private List<Sprite> equipSlotSprite = new List<Sprite>();
    [SerializeField]
    private List<Image> slotImg = new List<Image>();
    public const int w = 4;
    public const int h = 4;

    public int equipSlotSize = 0;

    private void Update()
    {
        for (int idx = 0; idx < w * h; idx++)
        {
            Image sImage = slotImg[idx];
            if(sImage != null)
            {
                int x = idx % w;
                int y = idx / w;
                int imgIdx = 0;
                if (x - 1 < 0)
                {
                    if (y - 1 < 0)
                        imgIdx = 0;
                    else if (y + 1 >= h)
                        imgIdx = 6;
                    else
                        imgIdx = 3;
                }
                else if (x + 1 >= w)
                {
                    if (y - 1 < 0)
                        imgIdx = 2;
                    else if (y + 1 >= h)
                        imgIdx = 8;
                    else
                        imgIdx = 5;
                }
                else
                {
                    if (y - 1 < 0)
                        imgIdx = 1;
                    else if (y + 1 >= h)
                        imgIdx = 7;
                    else
                        imgIdx = 4;
                }

                if(equipSlotSize <= idx)
                    sImage.sprite = slotSprite[imgIdx];
                else
                    sImage.sprite = equipSlotSprite[imgIdx];
            }
        }
    }
}
