using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Follow : SerializedMonoBehaviour
{
    [Header("�����ø� �̵�")]
    public bool initMove;

    [Header("���� ������Ʈ")]
    public GameObject followObj;

    [Header("������Ʈ ũ�� �ʱ�ȭ")]
    public bool initSize;

    [Header("���󰡴� ������Ʈ�� Transform ��ǥ�踦 ����ϴ°�")]
    public bool isTransfrom = true;

    [Header("���� ������Ʈ�� Transform ��ǥ�踦 ����ϴ°�")]
    public bool atTransfrom = false;

    [Header("�ӵ��� �����°�")]
    public bool hasSpeed = false;

    [ShowIf("hasSpeed")]
    public float speedValue = 10;

    #region[Start]
    private void Start()
    {
        if (initSize)
            transform.localScale = new Vector3(1, 1, 1);
        MoveObject();
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (initMove)
            return;
        MoveObject();
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
        if (!initMove)
            return;
        MoveObject();
    }
    #endregion

    #region[MoveObject]
    private void MoveObject()
    {
        if (followObj == null)
            return;

        if (isTransfrom)
        {
            if (hasSpeed)
            {
                if (atTransfrom)
                {
                    if (Vector2.Distance(transform.position, followObj.transform.position) < speedValue * Time.deltaTime)
                        transform.position = followObj.transform.position;
                    else
                    {
                        Vector2 dic = followObj.transform.position - transform.position;
                        dic = dic.normalized;

                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;
                    }
                }
                else
                {
                    if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(followObj.transform.position)) < speedValue * Time.deltaTime)
                        transform.position = Camera.main.ScreenToWorldPoint(followObj.transform.position);
                    else
                    {
                        Vector2 dic = Camera.main.ScreenToWorldPoint(followObj.transform.position) - transform.position;
                        dic = dic.normalized;

                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;


                    }
                }
            }
            else
            {
                if (atTransfrom)
                {
                    float z = transform.position.z;
                    transform.position = new Vector3(followObj.transform.position.x, followObj.transform.position.y, z);
                }
                else
                {
                    float z = transform.position.z;
                    transform.position = Camera.main.ScreenToWorldPoint(followObj.transform.position);
                    transform.position = new Vector3(transform.position.x, transform.position.y, z);
                }
            }
        }
        else
        {
            if (hasSpeed)
            {
                if (atTransfrom)
                {
                    if (Vector2.Distance(transform.position, Camera.main.WorldToScreenPoint(followObj.transform.position)) < speedValue * Time.deltaTime)
                        transform.position = Camera.main.WorldToScreenPoint(followObj.transform.position);
                    else
                    {
                        Vector2 dic = Camera.main.WorldToScreenPoint(followObj.transform.position) - transform.position;
                        dic = dic.normalized;
                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;
                    }
                }
                else
                {

                    if (Vector2.Distance(transform.position, followObj.transform.position) < speedValue * Time.deltaTime)
                        transform.position = followObj.transform.position;
                    else
                    {
                        Vector2 dic = followObj.transform.position - transform.position;
                        dic = dic.normalized;

                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (atTransfrom)
                    transform.position = Camera.main.WorldToScreenPoint(followObj.transform.position);
                else
                    transform.position = followObj.transform.position;
            }
        }
    }
    #endregion
}