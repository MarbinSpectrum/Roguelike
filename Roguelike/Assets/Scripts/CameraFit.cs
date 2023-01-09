using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �ػ󵵿� ���� ī�޶��� ũ�⸦ �����մϴ�.
////////////////////////////////////////////////////////////////////////////////
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [SerializeField] private float sceneWidth = 10;
    [SerializeField] private float sceneHeight = 10;
    [SerializeField] private float baseSceneWidth = 1080;
    [SerializeField] private float baseSceneHeight = 1920;
    [SerializeField] private Camera _camera;

    private void Update()
    {
        float rate = baseSceneWidth / baseSceneHeight;
        if (rate > (float)Screen.width / (float)Screen.height)
        {
            //���̰� ���� ����
            //�ʺ� �������� �ػ󵵸� �����.
            float unitsPerPixel = sceneWidth / Screen.width;
            _camera.orthographicSize = unitsPerPixel * Screen.height;
        }
        else
        {
            //�ʺ� ���� ����
            //���� �������� �ػ󵵸� �����.
            _camera.orthographicSize = sceneHeight;
        }
    }
}

