using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Obj", menuName = "ScriptableObjects/Obj", order = 1)]
public class ObjData : ScriptableObject
{
    public Obj obj;
    [PreviewField]
    public Sprite objSprite;
}
