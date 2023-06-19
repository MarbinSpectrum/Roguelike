using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Mgr : SerializedMonoBehaviour
{
    public static BulletManager         bulletMgr       { get => BulletManager.instance;    }
    public static CharacterManager      characterMgr    { get => CharacterManager.instance; }
    public static ChestManager          chestMgr        { get => ChestManager.instance;     }
    public static EtcObjManager         etcObjMgr       { get => EtcObjManager.instance;    }
    public static GameManager           gameMgr         { get => GameManager.instance;      }
    public static InventoryManager      inventoryMgr    { get => InventoryManager.instance; }
    public static ItemManager           itemMgr
    {
        get
        {
            ItemManager itemMgr = ItemManager.instance;
            itemMgr.Init();
            return itemMgr;
        }
    }
    public static JarManager            jarMgr          { get => JarManager.instance;       }
    public static LanguageManager       languageMgr     { get => LanguageManager.instance;  }
    public static MonsterManager monsterMgr
    {
        get
        {
            MonsterManager monsterMgr = MonsterManager.instance;
            monsterMgr.Init();
            return monsterMgr;
        }
    }
    public static ObjManager            objMgr
    {
        get
        {
            ObjManager objMgr = ObjManager.instance;
            objMgr.Init();
            return objMgr;
        }
    }
    public static ShopManager           shopMgr         { get => ShopManager.instance;      }
    public static SoundManager          soundMgr        { get => SoundManager.instance;     }
    public static StageManager          stageMgr
    {
        get
        {
            StageManager stageMgr = StageManager.instance;
            stageMgr.Init();
            return stageMgr;
        }
    }
    public static TorchManager          torchMgr        { get => TorchManager.instance;     }
    public static UIEffectManager       uIEffectMgr     { get => UIEffectManager.instance;  }
}
