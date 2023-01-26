using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class UpdateSlotCategory : SerializedMonoBehaviour
{
    [SerializeField]
    private List<GameObject> offslot = new List<GameObject>();
    [SerializeField]
    private List<GameObject> onslot = new List<GameObject>();

    private int ActCategory;
    [ShowInInspector,PropertyOrder(-1)]
    public int actCategory
    {
        get { return ActCategory; }
        set
        {
            if (value < 0)
                ActCategory = 0;
            else if (value >= 3)
                ActCategory = 2;
            else
                ActCategory = value;

            foreach (GameObject obj in offslot)
                obj.SetActive(true);
            foreach (GameObject obj in onslot)
                obj.SetActive(false);
            offslot[actCategory].SetActive(false);
            onslot[actCategory].SetActive(true);

            TotalUI totalUI = TotalUI.instance;
            if(totalUI != null)
            {
                if (actCategory == 0)
                    totalUI.UpdateInventory(ItemType.Etc);
                else if (actCategory == 1)
                    totalUI.UpdateInventory(ItemType.Weapon);
                else if (actCategory == 2)
                    totalUI.UpdateInventory(ItemType.Armor);
            }
        }
    }

    public void RunCategory(int pCategoryNum)
    {
        actCategory = pCategoryNum;
    }
}
