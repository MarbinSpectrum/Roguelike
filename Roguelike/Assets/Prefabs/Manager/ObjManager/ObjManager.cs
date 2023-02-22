using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 오브젝트의 정보를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class ObjManager : DontDestroySingleton<ObjManager>
{
    private Dictionary<Obj, ObjData> objDatas;

    [SerializeField]
    private List<ObjData> objDataList = new List<ObjData>();

    private void Init()
    {
        if (objDatas == null)
        {
            objDatas = new Dictionary<Obj, ObjData>();
            foreach (ObjData objData in objDataList)
            {
                objDatas[objData.obj] = objData;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pObj에 해당하는 Sprite를 반환한다.
    ////////////////////////////////////////////////////////////////////////////////
    public Sprite GetSprite(Obj pObj)
    {
        Init();

        if(objDatas.ContainsKey(pObj))
        {
            ObjData objData = objDatas[pObj];
            Sprite objSprite = objData.objSprite;

            return objSprite;
        }
        return null;
    }


}
