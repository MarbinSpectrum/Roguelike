
public enum Tile
{
    Null_Tile = 0,

    Stone_Floor_0 = 1,


    Stone_Wall_0 = 101,
    Stone_Wall_1 = 102,
    Stone_Wall_2 = 103,
    Stone_Wall_3 = 104,
    Stone_Wall_4 = 105,
    Stone_Wall_5 = 106,
    Stone_Wall_6 = 107,
    Stone_Wall_7 = 108,
    Stone_Wall_8 = 109,
    Stone_Wall_9 = 110,
    Stone_Wall_10 = 111,
    Stone_Wall_11 = 112,
    Stone_Wall_12 = 113,
    Stone_Wall_13 = 114,
    Stone_Wall_14 = 115,
}

public enum TileType
{
    Floor = 1,

    Wall = 101,
}

public enum RoomType
{
    Type0 = 0, //���ʿ��� ���� �ִ� Ÿ��
    Type1 = 1, //�� ������� �����ִ� Ÿ��
    Type2 = 2, //�� ������� �����ִ� Ÿ��
    Type3 = 3, //�� ������� �����ִ� Ÿ��
    Type4 = 4, //�� ������� �����ִ� Ÿ��
}

public enum Obj
{
    Null = 0,

    StartPos = 100,


    Slime = 1000,
}

public enum ButtonInput
{
    None,
    Left,
    Right,
    Up,
    Down,
    Attack
}
