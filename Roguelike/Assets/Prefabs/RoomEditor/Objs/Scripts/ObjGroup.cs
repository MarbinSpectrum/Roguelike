using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 기타 오브젝트의 그룹을 관리합니다.
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class ObjGroup : SerializedMonoBehaviour
{
    [HideInInspector]
    public float tileWidth;
    [HideInInspector]
    public float tileHeight;
    [HideInInspector]
    public Vector2 startPos;

    [Title("RequireData")]
    [SerializeField]
    private List<EditorObj> objs = new List<EditorObj>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        int idx = 0;
        for (int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                if (objs.Count <= idx)
                    continue;
                EditorObj eObj = objs[idx];
                if (eObj == null)
                    continue;
                eObj.gameObject.SetActive(true);
                eObj.transform.position
                    = new Vector3(
                        startPos.x + x * tileWidth,
                        startPos.y + y * tileHeight, 0);
                idx++;
            }
        }


        for (; idx < objs.Count; idx++)
        {
            EditorObj eObj = objs[idx];
            if (eObj == null)
                continue;
            eObj.gameObject.SetActive(false);
        }
    }

    public List<Obj> GetObjs()
    {
        List<Obj> roomObjData = new List<Obj>();

        int idx = 0;
        for (int x = 0; x < RoomData.roomSize; x++)
        {
            for (int y = 0; y < RoomData.roomSize; y++)
            {
                if (objs.Count <= idx)
                    continue;
                EditorObj eObj = objs[idx];
                if (eObj == null)
                    continue;
                roomObjData.Add(eObj.obj);

                idx++;
            }
        }

        return roomObjData;
    }
}
