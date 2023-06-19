using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObjects/Monster", order = 1)]
public class MonsterData : ScriptableObject
{
    public MonsterObj monsterObj;

    [PreviewField]
    public Sprite previewSprite;
    public Obj monsterType;

    //체력
    public uint hp;

    //데미지
    public uint damage;

    //공격딜레이
    public uint attackDelay;

    //이동딜레이
    public uint moveDelay;

    //인식범위
    public uint range;

    //경험치
    public uint exp;
}
