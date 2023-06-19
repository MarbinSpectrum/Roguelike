using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 총기작업대 오브젝트
////////////////////////////////////////////////////////////////////////////////
public class GunBenchObj : MonoBehaviour
{
    [SerializeField]
    private GameObject act;
    [SerializeField]
    private GameObject unAct;

    private void Start()
    {
        ActObj(GameManager.gunBenchAct);
    }

    private void ActObj(bool state)
    {
        if(state)
        {
            act.SetActive(true);
            unAct.SetActive(false);
        }
        else
        {
            act.SetActive(false);
            unAct.SetActive(true);
        }
    }
}
