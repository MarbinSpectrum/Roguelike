using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
/// : 항아리 객체들의 정보를 관리하는 매니저
////////////////////////////////////////////////////////////////////////////////
public class JarManager : FieldObjectSingleton<JarManager>
{
    public List<Jar> jarPrefabs = new List<Jar>();
    private HashSet<Vector2Int> jarPos = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, Jar> jarObjs = new Dictionary<Vector2Int, Jar>();
    private Dictionary<int, Queue<Jar>> jarQueue = new Dictionary<int, Queue<Jar>>();

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos위치에 항아리가 생성될곳이다.
    /// : jarPos에 담겨있는 값을 참조해서
    /// : runCreateJarObj에서 실질적으로 항아리를 생성한다.
    ////////////////////////////////////////////////////////////////////////////////
    public void AddJarPos(Vector2Int pPos)
    {
        jarPos.Add(pPos);
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pPos위치에 있는 항아리를 가져온다.
    ////////////////////////////////////////////////////////////////////////////////
    public Jar GetJarObj(Vector2Int pPos)
    {
        if (jarObjs.ContainsKey(pPos))
            return jarObjs[pPos];
        return null;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : pX,pY위치에 항아리가 있는지 검사한다.
    ////////////////////////////////////////////////////////////////////////////////
    public bool IsJar(int pX,int pY)
    {
        if (jarPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    ////////////////////////////////////////////////////////////////////////////////
    /// : 랜덤한 항아리를 생성한다.
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
    /// : pPos에 위치한 항아리를 제거한다.
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
    /// : 모든 항아리를 제거
    ////////////////////////////////////////////////////////////////////////////////
    public void RemoveAll_JarObj()
    {
        foreach (KeyValuePair<int, Queue<Jar>> jarQ in jarQueue)
        {
            Queue<Jar> tempQueue = new Queue<Jar>();
            while (jarQ.Value.Count > 0)
            {
                Jar jarObj = jarQ.Value.Dequeue();
                jarObj.gameObject.SetActive(false);
                jarObj.Init();
                tempQueue.Enqueue(jarObj);
            }
            while (tempQueue.Count > 0)
            {
                Jar jarObj = tempQueue.Dequeue();
                jarQ.Value.Enqueue(jarObj);
            }
        }

        foreach (Vector2Int jPos in jarPos)
        {
            Jar jarObj = GetJarObj(jPos);
            if (jarObj == null)
                continue;
            jarObj.gameObject.SetActive(false);
            jarQueue[jarObj.jarIdx].Enqueue(jarObj);
        }
        jarPos.Clear();
        jarObjs.Clear();
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
