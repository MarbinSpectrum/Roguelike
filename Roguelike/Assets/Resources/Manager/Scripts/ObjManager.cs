using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : ������Ʈ�� ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class ObjManager : DontDestroySingleton<ObjManager>
{
    private Dictionary<Obj, ObjData> objDatas;

    [SerializeField]
    private List<ObjData> objDataList = new List<ObjData>();

    public void Init()
    {
        if (objDatas == null)
        {
            InitData();
        }
    }

    [Button("InitData")]
    private void InitData()
    {
        objDatas = new Dictionary<Obj, ObjData>();
        foreach (ObjData objData in objDataList)
        {
            objDatas[objData.obj] = objData;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pObj�� �ش��ϴ� Sprite�� ��ȯ�Ѵ�.
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
