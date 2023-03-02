
public enum Tile
{
    Null_Tile = 0,

    Stone_Floor_0 = 1,
    Stone_Floor_1,
    Stone_Floor_2,
    Stone_Floor_3,

    Stone_Stair_0 = 30,


    Stone_Wall_0 = 101,
    Stone_Wall_1,
    Stone_Wall_2,
    Stone_Wall_3,
    Stone_Wall_4,
    Stone_Wall_5,
    Stone_Wall_6,
    Stone_Wall_7,
    Stone_Wall_8,
    Stone_Wall_9,
    Stone_Wall_10,
    Stone_Wall_11,
    Stone_Wall_12,
    Stone_Wall_13,
    Stone_Wall_14,


    Mat_Floor_0 = 201,
    Mat_Floor_1,
    Mat_Floor_2,
    Mat_Floor_3,
    Mat_Floor_4,
    Mat_Floor_5,
    Mat_Floor_6,
    Mat_Floor_7,
    Mat_Floor_8,
    Mat_Floor_9,
    Mat_Floor_10,
    Mat_Floor_11,
    Mat_Floor_12,
    Mat_Floor_13,
    Mat_Floor_14,
    Mat_Floor_15,
    Mat_Floor_16,
    Mat_Floor_17,
    Mat_Floor_18,
    Mat_Floor_19,
    Mat_Floor_20,
    Mat_Floor_21,
    Mat_Floor_22,
    Mat_Floor_23,
    Mat_Floor_24,
    Mat_Floor_25,
    Mat_Floor_26,
    Mat_Floor_27,
    Mat_Floor_28,


    Wood_Floor_0 = 401,

    Wood_Wall_0 = 501,
    Wood_Wall_1,
    Wood_Wall_2,
    Wood_Wall_3,
    Wood_Wall_4,
    Wood_Wall_5,
    Wood_Wall_6,
    Wood_Wall_7,
    Wood_Wall_8,
    Wood_Wall_9,
    Wood_Wall_10,
    Wood_Wall_11,
    Wood_Wall_12,
    Wood_Wall_13,
    Wood_Wall_14,
}

public enum TileType
{
    Null = 0,

    Stone_Floor = 1,
    Wood_Floor = 2,
    Mat_Floor = 3,

    Stone_Stair = 51,

    Stone_Wall = 101,
    Wood_Wall = 102,
}

public enum RoomType1
{
    Type0 = 0, //���ʿ��� ���� �ִ� Ÿ��
    Type1 = 1, //�� ������� �����ִ� Ÿ��
    Type2 = 2, //�� ������� �����ִ� Ÿ��
    Type3 = 3, //�� ������� �����ִ� Ÿ��
    Type4 = 4, //�� ������� �����ִ� Ÿ��
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

    StoneDoor = 205,

    TorchLight = 500,

    Jar = 600,
    Chest_Normal_Pos = 610,
    Chest_Normal = 615,

    GunBench = 700,

    ShopObj = 750,



    Slime = 1000,

    Tentacle = 2000,
}

public enum Item
{
    Null = 0,

    Coin = 100,
    ScrapMetal = 105,  


    NormalGun = 200, //�⺻ ����
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
    Null                = 0,

    Curse               = 10,           //���־����� �������� ����.

    Pow                 = 100,          //���ݷ�
    Balance             = 150,          //�뷱��
    CriRate             = 200,          //ũ��Ƽ�� Ȯ��(%)
    CriDmg              = 300,          //ũ��Ƽ�� ������(%)

    Hp                  = 400,
    AddExp              = 500,          //�߰� ����ġ(%)
    AddGold             = 600,          //�߰� ���(%)
    HitDamage           = 700,          //�޴� ������ �߰�
    HitDie              = 720,          //������ ������ ���
    Shield              = 800,          //�������� ���尡 �߰�(�ش� ������ ������ ���尡 ����)


    CanUpgradePow       = 5100,         //���ݷ� ���׷��̵� ����
    CanUpgradeBalance   = 5150,         //�뷱�� ���׷��̵� ����
    CanUpgradeCriRate   = 5200,         //ũ��Ƽ�� Ȯ��(%) ���׷��̵� ����
    CanUpgradeCriDmg    = 5250,         //ũ��Ƽ�� ������(%) ���׷��̵� ����

    IsUpgrade           = 6000,         //�ش� ����� ��ȭ�� Ƚ��






    Heal                = 10000,        //���� ü��ȸ��
}

public enum StageName
{
    Stage1_1 = 10100,
    ShelterMap1_1 = 10101,
    Stage1_2 = 10200,

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