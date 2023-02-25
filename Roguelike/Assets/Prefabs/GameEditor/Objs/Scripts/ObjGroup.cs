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
    [SerializeField]
    private EditorObj objPrefabs;
    public RoomEditor roomEditor;
    public CustomMapEditor customMapEditor;
    private uint w;
    private uint h;

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateObjGroup();
    }

    public void UpdateObjGroup()
    {
        if (roomEditor != null)
        {
            w = RoomData.roomSize;
            h = RoomData.roomSize;
        }
        else if (customMapEditor != null)
        {
            w = customMapEditor.width;
            h = customMapEditor.height;
        }
        else
        {
            return;
        }

        int idx = 0;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (objs.Count <= idx)
                {
                    EditorObj newObj = Instantiate(objPrefabs);
                    newObj.transform.parent = transform;
                    objs.Add(newObj);
                }
                EditorObj eObj = objs[idx];
                if (eObj == null)
                    continue;
                eObj.gameObject.SetActive(true);
                eObj.transform.position
                    = new Vector3(
                        startPos.x + x * tileWidth,
                        startPos.y + y * tileHeight, 0);
                eObj.transform.parent = transform;
                eObj.transform.localScale = new Vector3(1, 1, 1);
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
        if (roomEditor != null)
        {
            w = RoomData.roomSize;
            h = RoomData.roomSize;
        }
        else if (customMapEditor != null)
        {
            w = customMapEditor.width;
            h = customMapEditor.height;
        }
        else
        {
            return null;
        }

        List<Obj> roomObjData = new List<Obj>();

        int idx = 0;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
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

    ////////////////////////////////////////////////////////////////////////////////
    /// : 맵을 초기화한다.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Clear", ButtonSizes.Large)]
    public void ExportData()
    {
        foreach(EditorObj editorObj in objs)
        {
            editorObj.obj = Obj.Null;
        }
    }
}
