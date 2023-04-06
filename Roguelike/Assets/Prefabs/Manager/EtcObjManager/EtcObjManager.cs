using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcObjManager : FieldObjectSingleton<EtcObjManager>
{
    [SerializeField]
    protected GameObject startStairsObj;
    [SerializeField]
    protected GameObject exitStairsObj;

    [SerializeField]
    protected GameObject stoneDoorObj;

    [SerializeField]
    protected GameObject gunBench;
    [SerializeField]
    protected GameObject shopObj;

    [SerializeField]
    protected GameObject drumLight;

    public static void CreateEtcObj(Vector2Int pPos, Obj pObj)
    {
        GameObject etcObj = null;
        switch (pObj)
        {
            case Obj.StartPos:
                {
                    etcObj = Instantiate(instance.startStairsObj);
                }
                break;
            case Obj.EndPos:
                {
                    etcObj = Instantiate(instance.exitStairsObj);
                }
                break;

            case Obj.StoneDoor:
                {
                    etcObj = Instantiate(instance.stoneDoorObj);
                }
                break;

            case Obj.GunBench:
                {
                    etcObj = Instantiate(instance.gunBench);
                }
                break;

            case Obj.ShopObj:
                {
                    etcObj = Instantiate(instance.shopObj);
                }
                break;

            case Obj.DrumLight:
                {
                    etcObj = Instantiate(instance.drumLight);
                }
                break;
        }
        if(etcObj != null)
        {
            Vector3 objPos = new Vector3(pPos.x * CreateMap.tileSize, pPos.y * CreateMap.tileSize, 0);
            etcObj.transform.position = objPos;
        }
    }
}
