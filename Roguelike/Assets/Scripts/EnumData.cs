
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

public enum RoomType1
{
    Type0 = 0, //한쪽에만 문이 있는 타입
    Type1 = 1, //─ 모양으로 문이있는 타입
    Type2 = 2, //┐ 모양으로 문이있는 타입
    Type3 = 3, //┨ 모양으로 문이있는 타입
    Type4 = 4, //┼ 모양으로 문이있는 타입
}

public enum RoomType2
{
    NormalRoom = 0,
    StartRoom = 1,
    EndRoom = 2,
}

public enum Obj
{
    Null = 0,

    StartPos = 100,
    EndPos = 120,

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

    Coolness_Ring = 600,
    Curse_Coolness_Ring = 610,
    Angry_Ring = 700,
    Curse_Angry_Ring = 710,
    Guardian_Ring = 800,
    Life_Ring = 900,
    Curse_Life_Ring = 1000,
    Gold_Ring = 1100,
    Silver_Ring = 1120,
    Leaf_Ring = 1200,
    Curse_Leaf_Ring = 1300,
    Skull_Ring = 1400,
    Curse_Skull_Ring = 1410,
    Wood_Ring = 1500,

    Potion = 10000
}

public enum ItemStat
{
    Null = 0,

    Pow = 100,              //공격력
    Balance = 150,          //밸런스
    CriPer = 200,           //크리티컬 확률(%)
    CriDmg = 300,           //크리티컬 데미지(%)

    Hp = 400,
    AddExp = 500,           //추가 경험치(%)
    AddGold = 600,          //추가 골드(%)
    HitDamage = 700,        //받는 데미지 추가
    Shield = 800,        //일정량의 쉴드가 추가(해당 아이템 고유의 쉴드가 존재)

    Heal = 10000
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