using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarManager : FieldObjectSingleton<JarManager>
{
    private HashSet<Vector2Int> jarPos = new HashSet<Vector2Int>();
    public List<Jar> jarPrefabs = new List<Jar>();
    private Dictionary<Vector2Int, Jar> jarObjs = new Dictionary<Vector2Int, Jar>();

    public void AddJarPos(Vector2Int pPos)
    {
        jarPos.Add(pPos);
    }

    public Jar GetJarObj(Vector2Int pPos)
    {
        if (jarObjs.ContainsKey(pPos))
            return jarObjs[pPos];
        return null;
    }

    public bool IsJar(int pX,int pY)
    {
        if (jarPos.Contains(new Vector2Int(pX, pY)))
            return true;
        return false;
    }

    public IEnumerator runCreateJarObj()
    {
        foreach(Vector2Int pos in jarPos)
        {
            int r = Random.Range(0, jarPrefabs.Count);
            Jar jarObj = Instantiate(jarPrefabs[r]);
            jarObj.pos = pos;
            jarObj.transform.position = 
                new Vector3(pos.x * CreateMap.tileSize, pos.y * CreateMap.tileSize, 0);
            jarObjs[pos] = jarObj;
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void RemoveJarObj(Vector2Int pPos)
    {
        Jar jarObj = GetJarObj(pPos);
        jarPos.Remove(pPos);
        jarObjs.Remove(pPos);
    }
}
