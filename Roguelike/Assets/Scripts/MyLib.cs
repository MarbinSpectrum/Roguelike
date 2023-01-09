using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace MyLib
{
    public static class Exception
    {
        #region[배열 범위초과 검사]
        public static bool IndexOutRange<T>(int x, int y, T[,] array)
        {
            if (x >= array.GetLength(0) || x < 0 || y >= array.GetLength(1) || y < 0)
                return false;
            return true;
        }

        public static bool IndexOutRange<T>(Vector2Int v, T[,] array)
        {
            return IndexOutRange<T>(v.x, v.y, array);
        }

        public static bool IndexOutRange<T>(int a, List<T> array)
        {
            if (array == null || a >= array.Count || a < 0)
                return false;
            return true;
        }

        public static bool IndexOutRange<T>(int a, T[] array)
        {
            if (a >= array.GetLength(0) || a < 0)
                return false;
            return true;
        }
        #endregion
    }

    public static class AreaCheck
    {
        #region[범위 내부인지 검사]
        public static bool RectIn(Vector2 pos,Rect rect)
        {
            if (rect.x > pos.x || rect.x + rect.width < pos.x || rect.y < pos.y || rect.y - rect.height > pos.y)
                return false;
            return true;
        }

        public static bool RectIn(Vector2 pos, RectInt rect)
        {
            return RectIn(pos, new Rect(rect.x, rect.y, rect.width, rect.height));
        }
        #endregion
    }

    public static class Algorithm
    {
        #region[Next_Permutation]
        public static bool Next_Permutation<T>(List<T> list) where T : IComparable
        {
            Action<int, int> Swap = (int idx1, int idx2) => { T temp = list[idx1]; list[idx1] = list[idx2]; list[idx2] = temp; };
            int a = 0, b = 0, p = 0; //p : pivot
            for (int i = list.Count - 2; i >= 0; --i)
                if (list[i].CompareTo(list[i + 1]) < 0)
                {
                    a = i;
                    p = i + 1;

                    for (int j = list.Count - 1; j >= 0; --j)
                        if (list[a].CompareTo(list[j]) < 0)
                        {
                            b = j;
                            break;
                        }

                    Swap(a, b);

                    for (int j = 0; j < (list.Count - p) / 2; j++)
                        Swap(j + p, list.Count - j - 1);

                    return true;
                }
            // 이미 순서대로 정렬되어 있음 => 역순으로 뒤집
            for (int i = 0; i < list.Count / 2; i++)
                Swap(i, list.Count - i - 1);
            return false;
        }
        #endregion

        #region[Swap]
        public static void Swap<T>(ref T a,ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        #endregion

        #region[Shuffle]
        public static void Shuffle<T>(ref List<T> list)
        {
            //list에 있는 데이터를 섞는다.
            //랜덤하게 인데스(a,b) 두개를 정하고
            //해당 인덱스에 해당하는 값을 교환한다.
            //해당 과정을 list길이*10번 만큼 반복한다.
            for (int i = 0; i < list.Count * 10; i++)
            {
                int a = UnityEngine.Random.Range(0, list.Count);
                int b = UnityEngine.Random.Range(0, list.Count);

                T temp = list[a];
                list[a] = list[b];
                list[b] = temp;
            }
        }
        #endregion

        #region[CreateRandomList]
        //1~N중에서 중복하지 않는 m개를 뽑는다.
        public static List<int> CreateRandomList(int n,int m)
        {
            int[] tree = new int[n+1];
            List<int> temp = new List<int>();

            int Sum(int i)
            {
                int ans = 0;
                while (i > 0)
                {
                    ans += tree[i];
                    i -= (i & -i);
                }
                return ans;
            }

            void Update(int i, int num)
            {
                while (i <= n)
                {
                    tree[i] += num;
                    i += (i & -i);
                }
            }

            for (int i = 1; i <= n; i++)
                Update(i, 1);

            for (int i = n; i > n - m; i--)
            {
                int rand = UnityEngine.Random.Range(0, i);

                int left = 1;
                int right = i;
                while (left < right)
                {
                    int mid = (left + right) / 2;
                    if (Sum(mid) >= rand)
                        right = mid;
                    else
                        left = mid + 1;
                }
                temp.Add(right);
                Update(right, -1);
            }

            return temp;
        }
        #endregion

        #region[MakeMaze]
        public static bool[,] MakeMaze(uint pWidth,uint pHeight)
        {
            uint mazeW = pWidth * 2 + 1;
            uint mazeH = pHeight * 2 + 1;
            bool[,] isWall = new bool[mazeW, mazeH];
            for (int x = 0; x < mazeW; x++)
            {
                for (int y = 0; y < mazeH; y++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        isWall[x, y] = true;
                    else
                        isWall[x, y] = false;
                }
            }

            for (int x = 0; x < mazeW; x++)
            {
                for (int y = 0; y < mazeH; y++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == mazeH - 2)
                    {
                        isWall[x + 1, y] = false;
                        continue;
                    }

                    if (x == mazeW - 2)
                    {
                        isWall[x, y + 1] = false;
                        continue;
                    }

                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        isWall[x + 1, y] = false;
                    }
                    else
                    {
                        isWall[x, y + 1] = false;
                    }
                }
            }

            for (int x = 0; x < mazeW; x++)
                for (int y = 0; y < mazeH; y++)
                    if (x == 0 || y == 0 || x == mazeW - 1 || y == mazeH - 1)
                    {
                        //가장자리 길막기
                        isWall[x, y] = true;
                    }

            return isWall;
        }
        #endregion

        #region[A*]


        //pFrom에서 pTo로 이동하는 경로를 구한다.
        public struct AStarRoute
        {
            public int w;
            public Vector2Int parents;
        }


        public static List<Vector2Int> AstartRoute(Vector2Int pFrom, Vector2Int pTo, bool[,] pIsWall)
        {
            List<Vector2Int> route = null;
            List<Vector2Int> OpenList = new List<Vector2Int>(); //열린리스트
            OpenList.Add(pFrom);

            int arrayW = pIsWall.GetLength(0);
            int arrayH = pIsWall.GetLength(1);

            HashSet<Vector2Int> CloseList = new HashSet<Vector2Int>(); //닫힌리스트
            AStarRoute[,] Area = new AStarRoute[arrayW, arrayH];
            bool explore = false;
            int MinV = 0;

            while (OpenList.Count > 0)
            {
                int MaxV = int.MaxValue;

                //4방향
                int[,] offset = { { 0, 1 }, { 0, -1 }, { -1, 0 }, { 1, 0 }};

                Vector2Int nowPos = OpenList[MinV];

                for (int i = 0; i < 4; i++)
                {
                    int ax = nowPos.x + offset[i, 0];
                    int ay = nowPos.y + offset[i, 1];
                    if (ax < 0 || ay < 0 || ax >= arrayW || ay >= arrayH)
                        continue;

                    if (pIsWall[ax, ay] == false && CloseList.Contains(new Vector2Int(ax, ay)) == false)
                    {
                        if (OpenList.Contains(new Vector2Int(ax, ay)))
                        {
                            if (Area[ax, ay].w > Area[nowPos.x, nowPos.y].w + 10)
                            {
                                Area[ax, ay].w = Area[nowPos.x, nowPos.y].w + 10;
                                Area[ax, ay].parents = nowPos;
                            }
                        }
                        else
                        {
                            OpenList.Add(new Vector2Int(ax, ay));
                            Area[ax, ay].w = Area[nowPos.x, nowPos.y].w + 10;
                            Area[ax, ay].parents = nowPos;
                        }
                    }

                    if (OpenList.Contains(pTo))
                    {
                        explore = true;
                        break;
                    }
                }

                if (explore)
                    break;

                for (int i = 0; i < OpenList.Count; i++)
                {
                    if (i == MinV)
                    {
                        CloseList.Add(OpenList[MinV]);
                        OpenList.RemoveAt(i);
                        break;
                    }
                }

                if (OpenList.Count > 0)
                {
                    for (int i = 0; i < OpenList.Count; i++)
                    {
                        int tax = (int)Vector2.Distance(OpenList[i], pTo);
                        if (MaxV > tax + Area[OpenList[i].x, OpenList[i].y].w)
                        {
                            MaxV = tax + Area[OpenList[i].x, OpenList[i].y].w;
                            MinV = i;
                        }
                    }
                }
            }

            if (explore)
            {
                route = new List<Vector2Int>();
                int ax = pTo.x;
                int ay = pTo.y;

                while (ax != pFrom.x || ay != pFrom.y)
                {
                    route.Add(new Vector2Int(ax, ay));
                    int bx = ax;
                    int by = ay;
                    ax = Area[bx, by].parents.x;
                    ay = Area[bx, by].parents.y;
                }
            }

            return route;
        }
        #endregion
    }

    public static class Calculator
    {
        #region[CalculateHexagonPos]

        //블록의 크기를 고려해서 x,y에 해당하는 블록의 위치를 구한다.
        public static Vector2 CalculateHexagonPos(float blockWidth, float blockHeight, int x, int y)
        {
            float resultY = y * blockHeight * 0.5f;
            float resultX = x * blockWidth;
            if (y % 2 != 1)
                resultX += blockWidth * 0.5f;

            return new Vector2(resultX, resultY);
        }
        #endregion

        #region[GetAroundHexagonPos]
        private static int[][,] hexagonAroundPos = new int[][,]
        {
            new int[,]{{0, 2}, {+1, +1}, {1, -1}, {0, -2}, { +0, -1}, {+0, +1}},
            new int[,]{{0, 2}, {+0, +1}, {0, -1}, {0, -2}, { -1, -1}, {-1, +1}},
        };

        //x,y주변의 블럭을 구해준다.
        public static List<Vector2Int> GetAroundHexagonPos(int pX, int pY)
        {
            int by = pY % 2; //y좌표에 따라서 주변 블록의 위치가 다르다.
            List<Vector2Int> aroundList = new List<Vector2Int>();
            for (int idx = 0; idx < 6; idx++)
            {
                int newX = pX + hexagonAroundPos[by][idx, 0];
                int newY = pY + hexagonAroundPos[by][idx, 1];
                aroundList.Add(new Vector2Int(newX, newY));
            }
            return aroundList;
        }
        #endregion

        #region[GetDicHeHexagonPos]
        public static Vector2Int GetDicHeHexagonPos(int pX, int pY,float pAngle)
        {
            int by = pY % 2; //y좌표에 따라서 주변 블록의 위치가 다르다.
            float angleR = 120;
            float angleL = 60;
            for (int idx = 0; idx < 6; idx++)
            {
                float minV = Mathf.Min(angleL, angleR);
                float maxV = Mathf.Max (angleL, angleR);
                if (minV <= pAngle && pAngle <= maxV)
                {
                    int newX = pX + hexagonAroundPos[by][idx, 0];
                    int newY = pY + hexagonAroundPos[by][idx, 1];
                    return new Vector2Int(newX, newY);
                }
                angleL -= 60;
                angleR -= 60;
                if(angleL < 0 || angleR < 0)
                {
                    angleL += 360;
                    angleR += 360;
                }
            }

            return Vector2Int.one * -1; ;
        }
        #endregion

        #region[GetAround8Pos]
        private static int[,] Around8Pos = new int[,]
        {
            {0, 1}, {+1, +1}, {1, 0}, {1, -1}, { 0, -1}, {-1, -1}, { -1, 0}, {-1, +1}
        };
        //x,y주변의 블럭을 구해준다.
        public static List<Vector2Int> GetAround8Pos(int pX, int pY)
        {
            List<Vector2Int> aroundList = new List<Vector2Int>();
            for (int idx = 0; idx < 8; idx++)
            {
                int newX = pX + Around8Pos[idx, 0];
                int newY = pY + Around8Pos[idx, 1];
                aroundList.Add(new Vector2Int(newX, newY));
            }
            return aroundList;
        }
        #endregion
    }

    public static class Json
    {
        #region[JSON 유틸]
        public static string ObjectToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static T JsonToOject<T>(string jsonData)
        {
            return JsonUtility.FromJson<T>(jsonData);
        }

        public static void CreateJsonFile(string createPath, string fileName, string jsonData)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }

        public static T LoadJsonFile<T>(string loadPath, string fileName)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }

        [System.Serializable]
        public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
        {
            [SerializeField]
            List<TKey> keys;
            [SerializeField]
            List<TValue> values;

            Dictionary<TKey, TValue> target;
            public Dictionary<TKey, TValue> ToDictionary() { return target; }

            public Serialization(Dictionary<TKey, TValue> target)
            {
                this.target = target;
            }

            public void OnBeforeSerialize()
            {
                keys = new List<TKey>(target.Keys);
                values = new List<TValue>(target.Values);
            }

            public void OnAfterDeserialize()
            {
                var count = Mathf.Min(keys.Count, values.Count);
                target = new Dictionary<TKey, TValue>(count);
                for (var i = 0; i < count; ++i)
                {
                    target.Add(keys[i], values[i]);
                }
            }
        }

        [System.Serializable]
        public class Serialization<T>
        {
            [SerializeField]
            List<T> target;
            public List<T> ToList() { return target; }

            public Serialization(List<T> target)
            {
                this.target = target;
            }
        }
        #endregion
    }

    public static class Action2D
    {
        #region[MoveTo]
        public static IEnumerator MoveTo(Transform target, Vector3 to, float duration)
        {
            Vector2 from = target.position;

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.smoothDeltaTime;
                target.position =
                    Vector2.Lerp(from, to, elapsed / duration);

                yield return null;
            }

            target.position = to;

            yield break;
        }
        #endregion
    }
}