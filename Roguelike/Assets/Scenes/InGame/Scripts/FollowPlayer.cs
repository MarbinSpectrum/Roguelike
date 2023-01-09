using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

////////////////////////////////////////////////////////////////////////////////
/// : �÷��̾�� �̵��ϴ� ������Ʈ
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
