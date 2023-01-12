using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObjects/Monster", order = 1)]
public class MonsterData : ScriptableObject
{
    public MonsterObj monsterObj;

    [PreviewField]
    public Sprite previewSprite;
    public Obj monsterType;

    //ü��
    public uint hp;

    //������
    public uint damage;

    //���ݵ�����
    public uint attackDelay;

    //�̵�������
    public uint moveDelay;

    //�νĹ���
    public uint range;

    //����ġ
    public uint exp;
}
