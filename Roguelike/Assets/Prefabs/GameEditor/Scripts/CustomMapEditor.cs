using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;
using MyLib;

////////////////////////////////////////////////////////////////////////////////
/// : Ÿ���� �׷��� �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
public class CustomMapEditor : SerializedMonoBehaviour
{
    [Title("Editor Setting")]
    [SerializeField]
    private float tileWidth;
    [SerializeField]
    private float tileHeight;
    [SerializeField]
    private Vector2 startPos;
    public uint width;
    public uint height;

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
        CustomMapData mapData = new CustomMapData(width, height, tileGroup, objGroup);
        string jsonData = Json.ObjectToJson(mapData);
        Json.CreateJsonFile(Application.dataPath, 
            "Resources/MapDatas/" + SceneManager.GetActiveScene().name, jsonData);
    }
}
