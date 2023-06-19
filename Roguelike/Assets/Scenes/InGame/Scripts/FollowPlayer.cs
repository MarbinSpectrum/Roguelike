using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 플레이어에게 이동하는 오브젝트
////////////////////////////////////////////////////////////////////////////////
public class FollowPlayer : Mgr
{
    [SerializeField]
    private bool followX = true;
    [SerializeField]
    private bool followY = true;
    [SerializeField]
    private Vector2 pivot;

    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void LateUpdate()
    {
        Vector3 pos = characterMgr.CharactorTransPos();
        pos.z = transform.position.z;
        if(followX == false)
        {
            pos.x = transform.position.x;
        }
        else
        {
            pos.x += pivot.x;
        }
        if (followY == false)
        {
            pos.y = transform.position.y;
        }
        else
        {
            pos.y += pivot.y;
        }
        transform.position = pos;
    }
}
