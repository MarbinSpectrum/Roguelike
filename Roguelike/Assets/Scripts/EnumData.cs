
public enum Tile
{
    Null_Tile = 0,

    Stone_Floor_0 = 1,
    Stone_Floor_1 = 2,
    Stone_Floor_2 = 3,
    Stone_Floor_3 = 4,

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
    Type0 = 0, //한쪽에만 문이 있는 타입
    Type1 = 1, //─ 모양으로 문이있는 타입
    Type2 = 2, //┐ 모양으로 문이있는 타입
    Type3 = 3, //┨ 모양으로 문이있는 타입
    Type4 = 4, //┼ 모양으로 문이있는 타입
}

public enum Obj
{
    Null = 0,

    StartPos = 100,

    TorchLight = 500,

    Jar = 600,
    Chest_Normal_Pos = 610,
    Chest_Normal = 615,

    Slime = 1000,

    Tentacle = 2000,
}

public enum Item
{
    Null = 0,

    Coin = 100,

    NormalGun = 200, //기본 장총

    Glock17 = 300,

    M4 = 400,

    MP133 = 500,

    Glasses_B = 600,

    Glasses_R = 700,

    Guardian_Ring = 800,
}

public enum ItemStat
{
    Null = 0,

    Pow = 100,
    Balance = 150,
    CriPer = 200,
    CriDmg = 300,

    Hp = 400,
    AddExp = 500,
    AddGold = 600,
}

public enum ItemType
{
    Etc = 100,
    Weapon = 200,
    Accessary = 300,
}

public enum ButtonInput
{
    None,
    Left,
    Right,
    Up,
    Down
}

public enum Language
{
    Korea = 100,
    English = 200,
}