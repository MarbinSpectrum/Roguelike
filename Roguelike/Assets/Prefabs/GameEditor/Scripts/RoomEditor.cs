using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : Ÿ���� �׷��� �����մϴ�.
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
    private RoomType1 roomType1;
    [SerializeField]
    private RoomType2 roomType2;

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
    /// : �����ͷ� ���� ���� Json���Ϸ� ��������.
    ////////////////////////////////////////////////////////////////////////////////
    [Button("Export Data", ButtonSizes.Large)]
    public void ExportData()
    {
        RoomData roomData = new RoomData(roomType1, roomType2, tileGroup, objGroup);
        string jsonData = Json.ObjectToJson(roomData);
        Json.CreateJsonFile(Application.dataPath, 
            "Resources/RoomDatas/" + SceneManager.GetActiveScene().name, jsonData);
    }
}
