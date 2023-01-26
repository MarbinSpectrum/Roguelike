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
    private List<Image> slotImg = new List<Image>();
    [SerializeField]
    private int w = 4;
    [SerializeField]
    private int h = 4;

    private void Update()
    {
        int idx = 0;
        foreach(Image sImage in slotImg)
        {
            if(sImage != null)
            {
                int x = idx % w;
                int y = idx / w;
                if (x - 1 < 0)
                {
                    if (y - 1 < 0)
                    {
                        sImage.sprite = slotSprite[0];
                    }
                    else if (y + 1 >= h)
                    {
                        sImage.sprite = slotSprite[6];
                    }
                    else
                    {
                        sImage.sprite = slotSprite[3];
                    }
                }
                else if (x + 1 >= w)
                {
                    if (y - 1 < 0)
                    {
                        sImage.sprite = slotSprite[2];
                    }
                    else if (y + 1 >= h)
                    {
                        sImage.sprite = slotSprite[8];
                    }
                    else
                    {
                        sImage.sprite = slotSprite[5];
                    }
                }
                else
                {
                    if (y - 1 < 0)
                    {
                        sImage.sprite = slotSprite[1];
                    }
                    else if (y + 1 >= h)
                    {
                        sImage.sprite = slotSprite[7];
                    }
                    else
                    {
                        sImage.sprite = slotSprite[4];
                    }
                }

                idx++;
            }
        }
    }
}
