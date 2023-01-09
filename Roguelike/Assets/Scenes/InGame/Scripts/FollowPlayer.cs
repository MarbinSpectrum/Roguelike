using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : 플레이어에게 이동하는 오브젝트
////////////////////////////////////////////////////////////////////////////////
public class FollowPlayer : SerializedMonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    /// : Update
    ////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        CatGirl catGirl = null;
        CharacterManager characterManager = CharacterManager.instance;
        catGirl = characterManager.character;

        if (catGirl == null)
            return;

        transform.parent = catGirl.transform;
        transform.localPosition = Vector3.zero;

    }
}
