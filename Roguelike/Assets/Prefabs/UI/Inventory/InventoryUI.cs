using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 인벤토리 활성/비활성
////////////////////////////////////////////////////////////////////////////////
public class InventoryUI : MonoBehaviour
{
    #region[private bool isRun]
    private bool IsRun;
    public bool isRun
    {
        get
        {
            return IsRun;
        }
        set
        {
            IsRun = value;
        }

    }
    #endregion

    [SerializeField]
    private GameObject uiObj;

    public void ActInventory()
    {
        isRun = !isRun;
        uiObj.SetActive(isRun);
    }
}
