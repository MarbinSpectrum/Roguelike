using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObjects/Monster", order = 1)]
public class MonsterData : ScriptableObject
{
    public MonsterObj monsterObj;

    [PreviewField]
    public Sprite previewSprite;
    public Obj monsterType;
    public uint hp;
    public uint damage;
    public uint moveDelay;
    public uint range;
}
