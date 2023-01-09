using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : 타일의 그룹을 관리합니다.
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class RoomEditor : SerializedMonoBehaviour
{
    [Title("Editor Setting")]
    [SerializeField]
    private float tileWidth;
    [SerializeField]
    private float tileHeight;
    [SerializeField]
    private Vector2 startPos;

    [Title("RoomData")]
    [SerializeField]
    private RoomType roomType;
    [SerializeField]
    private bool isStartRoom;

    [Title("RequireData")]
    [SerializeField]
    private TileGroup tileGroup = new TileGroup();
    [SerializeField]
    private ObjGroup objGroup = new ObjGroup();

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if(tileGroup != null)
        {
            tileGroup.tileWidth = tileWidth;
            tileGroup.tileHeight = tileHeight;
            tileGroup.startPos = startPos;
        }

        if (objGroup != null)
        {
            objGroup.tileWidth = tileWidth;
            objGroup.tileHeight = tileHeight;
            objGroup.startPos = startPos;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 에디터로 만든 방을 Json파일로 내보낸다.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Export Data", ButtonSizes.Large)]
    public void ExportData()
    {
        RoomData roomData = new RoomData(roomType, isStartRoom, tileGroup, objGroup);
        string jsonData = Json.ObjectToJson(roomData);
        Json.CreateJsonFile(Application.dataPath, 
            "Resources/RoomDatas/" + SceneManager.GetActiveScene().name, jsonData);
    }
}
