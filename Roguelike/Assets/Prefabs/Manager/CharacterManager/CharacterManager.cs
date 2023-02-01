using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : ĳ���͸� �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class CharacterManager : FieldObjectSingleton<CharacterManager>
{
    [SerializeField]
    private CatGirl catGirl;

    [HideInInspector]
    public CatGirl character;

    #region[public uint maxHp]
    private uint MaxHp;
    public uint maxHp
    {
        get
        {
            return MaxHp;
        }
    }
    #endregion

    #region[public uint nowHp]
    private uint NowHp;
    public uint nowHp
    {
        get
        {
            return NowHp;
        }
    }
    #endregion

    #region[public uint maxExp]
    private uint MaxExp;
    public uint maxExp
    {
        get
        {
            return MaxExp;
        }
    }
    #endregion

    #region[public uint nowExp]
    private uint NowExp;
    public uint nowExp
    {
        get 
        { 
            return NowExp; 
        }
    }
    #endregion

    #region[public uint nowLevel]
    private uint NowLevel;
    public uint nowLevel
    {
        get
        {
            return NowLevel;
        }
    }
    #endregion

    [SerializeField]
    private uint basePow;
    [SerializeField]
    private uint baseBalance;
    [SerializeField]
    private uint maxBalance;

    #region[public float baseCriPer]
    private float BaseCriPer;
    public float baseCriPer
    {
        get
        {
            return BaseCriPer;
        }
    }
    #endregion

    #region[public float baseCriDamage]
    private float BaseCriDamage;
    public float baseCriDamage
    {
        get
        {
            return BaseCriDamage;
        }
    }
    #endregion

    [HideInInspector]
    public bool canControl;

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY�� �ش��ϴ� ��ǥ�� ĳ���͸� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void CreateCatGirl(int pX, int pY)
    {
        if(character != null)
        {
            Destroy(character);
        }

        character = Instantiate(catGirl);

        character.gameObject.SetActive(true);
        character.SetPos(pX, pY);

        MaxExp = 5;
        NowExp = 0;
        MaxHp = 3;
        NowHp = MaxHp;
        NowLevel = 1;
        BaseCriDamage = 100;
        BaseCriPer = 50;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);
        totalUI.UpdateExp(maxExp, nowExp);

        canControl = true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �Է¸�� ��ư
    ////////////////////////////////////////////////////////////////////////////////
    public void CharactorInputButton(ButtonInput pButtonInput)
    {
        if (character == null)
            return;
        character.buttonInput = pButtonInput;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾ pDamage��ŭ ���ظ� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void Hit(uint pDamage)
    {
        if (nowHp > pDamage)
            NowHp -= pDamage;
        else
            NowHp = 0;

        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateHp(maxHp, nowHp);

        StartCoroutine(HitStunDelay());
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���ϵ�����
    ////////////////////////////////////////////////////////////////////////////////
    private IEnumerator HitStunDelay()
    {
        canControl = false;
        yield return new WaitForSeconds(0.5f);
        canControl = true;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ����ġ ȹ��
    ////////////////////////////////////////////////////////////////////////////////
    public void GetExp(uint pValue)
    {
        NowExp += pValue;
        if(nowExp >= maxExp)
        {
            //ȹ�����ġ�� �ִ����ġ�� �Ѱ�ٸ�
            //�����������Ѵ�.
            NowExp -= maxExp;

            LevelUp();
        }
        TotalUI totalUI = TotalUI.instance;
        totalUI.UpdateExp(maxExp, nowExp);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �÷��ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public void LevelUp()
    {
        //������ �÷��ְ� ���ݷ��� �÷��ش�.
        NowLevel += 1;
        basePow += 1;

        //���� �ִ� ����ġ�� �������ش�.
        MaxExp = (uint)(maxExp * 1.5f);

    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            character.buttonInput = ButtonInput.Left;
        else if (Input.GetKey(KeyCode.RightArrow))
            character.buttonInput = ButtonInput.Right;
        else if (Input.GetKey(KeyCode.UpArrow))
            character.buttonInput = ButtonInput.Up;
        else if (Input.GetKey(KeyCode.DownArrow))
            character.buttonInput = ButtonInput.Down;

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            character.buttonInput = ButtonInput.None;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            character.buttonInput = ButtonInput.None;
        else if (Input.GetKeyUp(KeyCode.UpArrow))
            character.buttonInput = ButtonInput.None;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            character.buttonInput = ButtonInput.None;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public uint GetTotalPow()
    {
        return basePow;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �����뷱���� �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public uint GetTotalBalance()
    {
        return baseBalance;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������������ �����ش�.
    ////////////////////////////////////////////////////////////////////////////////
    public int GetTotalDamage()
    {
        //  Pow*(balance/maxBalance) ~ Pow ���̷� �����ϰ� �������� �����ȴ�.
        int totalPow = (int)GetTotalPow();
        float per = GetTotalBalance() / (float)maxBalance;
        int minPow = (int)(per * totalPow);
        minPow = Mathf.Max(1, minPow);

        int damage = Random.Range(minPow, totalPow);
        return damage;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ Ȯ���� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriPer()
    {
        return baseCriPer;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ���� ġ��Ÿ �������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public float GetTotalCriDamage()
    {
        return baseCriDamage;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : �ش� �������� ũ��Ƽ�÷� ������� ���Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool CriticalProcess(ref int pDamage)
    {

        float damage = pDamage;
        if (Random.Range(0,100) < GetTotalCriPer())
        {
            //ũ��Ƽ�÷� ��������.
            //true�� ��ȯ�ϰ�
            //ũ��Ƽ�ø�ŭ�� �������� �÷��ش�.
            damage += damage * (GetTotalCriDamage() / 100f);
            pDamage = (int)damage;
            return true;
        }
        return false;
    }
}
