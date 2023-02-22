using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : �׾Ƹ� ��ü���� ������ �����ϴ� �Ŵ���
////////////////////////////////////////////////////////////////////////////////
public class JarManager : FieldObjectSingleton<JarManager>
{
    public List<Jar> jarPrefabs = new List<Jar>();
    private HashSet<Vector2Int> jarPos = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, Jar> jarObjs = new Dictionary<Vector2Int, Jar>();
    private Dictionary<int, Queue<Jar>> jarQueue = new Dictionary<int, Queue<Jar>>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos��ġ�� �׾Ƹ��� �����ɰ��̴�.
    /// : jarPos�� ����ִ� ���� �����ؼ�
    /// : runCreateJarObj���� ���������� �׾Ƹ��� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddJarPos(Vector2Int pPos)
    {
        jarPos.Add(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos��ġ�� �ִ� �׾Ƹ��� �����´�.
    ////////////////////////////////////////////////////////////////////////////////
    public Jar GetJarObj(Vector2Int pPos)
    {
        if (jarObjs.ContainsKey(pPos))
            return jarObjs[pPos];
        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY��ġ�� �׾Ƹ��� �ִ��� �˻��Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsJar(int pX,int pY)
    {
        if (jarPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ������ �׾Ƹ��� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public Jar CreateJarObj()
    {
        int randomIdx = Random.Range(0, jarPrefabs.Count);
        if (jarQueue.ContainsKey(randomIdx) == false)
            jarQueue[randomIdx] = new Queue<Jar>();

        Jar jar = null;
        if (jarQueue[randomIdx].Count > 0)
            jar = jarQueue[randomIdx].Dequeue();

        if (jar == null)
            jar = Instantiate(jarPrefabs[randomIdx]);
        jar.gameObject.SetActive(true);
        jar.jarIdx = randomIdx;
        jar.Init();

        return jar;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos�� ��ġ�� �׾Ƹ��� �����Ѵ�.
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveJarObj(Vector2Int pPos)
    {
        Jar jarObj = GetJarObj(pPos);
        if (jarObj == null)
            return;
        jarQueue[jarObj.jarIdx].Enqueue(jarObj);

        jarPos.Remove(pPos);
        jarObjs.Remove(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : ��� �׾Ƹ��� ����
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveAll_JarObj()
    {
        foreach (Vector2Int jPos in jarPos)
        {
            Jar jarObj = GetJarObj(jPos);
            if (jarObj == null)
                continue;
            jarObj.gameObject.SetActive(false);
            RemoveJarObj(jPos);
        }
    }

    public IEnumerator runCreateJarObj()
    {
        foreach(Vector2Int pos in jarPos)
        {
            Jar jarObj = CreateJarObj();
            jarObj.pos = pos;
            jarObj.transform.parent = transform;
            jarObj.transform.position = 
                new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);
            jarObjs[pos] = jarObj;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
