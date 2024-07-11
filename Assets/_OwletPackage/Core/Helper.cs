
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.EventSystems;

namespace Owlet
{
    public static class Helper
    {
        #region Camera
        static Camera _cam;
        public static Camera mainCam
        {
            get
            {
                if (_cam == null)
                {
                    _cam = Camera.main;
                }
                return _cam;
            }
        }

        public static RaycastHit2D RaycastCamera2D(Camera cam = null)
        {
            if (cam == null) cam = mainCam;

            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            return hit;
        }

        public static RaycastHit2D[] RaycastAllCamera2D(Vector3 position, Camera cam = null)
        {
            if (cam == null) cam = mainCam;

            RaycastHit2D[] hits = Physics2D.RaycastAll(cam.ScreenToWorldPoint(position), Vector2.zero);

            return hits;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            return mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        public static bool IsOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return true;

            if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                // Check if finger is over a UI element
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region SaveLoad
        public static void Save(string json, string path)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(json);
            writer.Close();
        }

        public static string Load(string path)
        {
            if (!File.Exists(path))
            {
                //LogError("File not exist");
                return "\0";
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                string json = reader.ReadToEnd();
                reader.Close();
                return json;
            }

        }
        #endregion

        #region Math
        public static float MapNumber(float value, float inMin, float inMax, float outMin, float outMax)
        {
            // Map the value from one range to another
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
        #endregion

        #region Isometric
        public static Vector3 CartToIso(Vector3Int coord, Vector3 offset)
        {
            float isoX = (coord.x - coord.y) / 2f;
            float isoY = (coord.x + coord.y + 2) / 4f + (coord.z + 1) / 4f;
            return new Vector3(isoX, isoY) + offset;
        }

        public static Vector2Int GetDirectionNormalized(Vector2Int start, Vector2Int end)
        {
            Vector2 direction = (Vector2)start - end;
            direction = direction.normalized;
            return new Vector2Int((int)(direction.x), (int)(direction.y));
        }
        #endregion

        #region Logger
        [HideInCallstack]
        public static void Log(string msg, GameObject obj = null)
        {
            Debug.Log(">>>" + msg, obj);
        }
        [HideInCallstack]
        public static void LogError(string msg, GameObject obj = null)
        {
            Debug.LogError(">>>" + msg, obj);
        }
        #endregion

        #region Editor
#if UNITY_EDITOR
        public static T EditorInstatiatePrefab<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : MonoBehaviour
        {

            string assetpath = PrefabUtility.
            GetPrefabAssetPathOfNearestInstanceRoot(prefab);
            GameObject go =
                AssetDatabase.LoadAssetAtPath(
                    assetpath, typeof(GameObject))
                    as GameObject;
            GameObject res = PrefabUtility.InstantiatePrefab(go) as GameObject;
            res.transform.SetPositionAndRotation(position, rotation);
            res.transform.parent = parent;
            return res.GetComponent<T>();

        }

        public static GameObject EditorInstatiatePrefab(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            string assetpath = PrefabUtility.
            GetPrefabAssetPathOfNearestInstanceRoot(prefab);
            GameObject go =
                AssetDatabase.LoadAssetAtPath(
                    assetpath, typeof(GameObject))
                    as GameObject;
            GameObject res = (PrefabUtility.InstantiatePrefab(go)) as GameObject;
            res.transform.position = position;
            res.transform.rotation = rotation;
            res.transform.parent = parent;
            return res;

        }
#endif
        #endregion

        #region Random
        public static float Random(this Vector2 vector2)
        {
            return UnityEngine.Random.Range(vector2.x, vector2.y);
        }
        public static int Random(this Vector2Int vector2)
        {
            return UnityEngine.Random.Range(vector2.x, vector2.y);
        }
        public static T Random<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public static List<T> GetUniqueRandomElements<T>(this List<T> inputList, int count)
        {
            List<T> inputListClone = new List<T>(inputList);
            Shuffle(inputListClone);
            count = Mathf.Min(count, inputList.Count);
            return inputListClone.GetRange(0, count);
        }
        public static void Shuffle<T>(this List<T> inputList)
        {
            for (int i = 0; i < inputList.Count - 1; i++)
            {
                T temp = inputList[i];
                int rand = UnityEngine.Random.Range(i, inputList.Count);
                inputList[i] = inputList[rand];
                inputList[rand] = temp;
            }
        }

        private static System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static T RandomWithWeight<T>(List<T> list, List<int> weights)
        {
            if (list.Count != weights.Count)
            {
                Debug.LogError("List and weight count doesn't match!");
                return Random(list);
            }

            int maxWeight = weights.Sum(x => x);
            int randomWeightValue = UnityEngine.Random.Range(0, maxWeight);
            int currentWeight = 0;

            int listCount = list.Count;

            for (int i = 0; i < listCount; i++)
            {
                currentWeight += weights[i];
                if (randomWeightValue < currentWeight) return list[i];
            }

            return Random(list); //fall back case if sth went wrong

        }

        #endregion

        #region Comparison
        public static int CompareVersions(string version1, string version2)
        {
            // Split the version strings into arrays of integers
            string[] v1Numbers = version1.Split('.');
            string[] v2Numbers = version2.Split('.');

            // Compare each segment of the version strings
            for (int i = 0; i < Math.Min(v1Numbers.Length, v2Numbers.Length); i++)
            {
                int v1 = int.Parse(v1Numbers[i]);
                int v2 = int.Parse(v2Numbers[i]);

                if (v1 < v2)
                    return -1;  // version1 is older
                else if (v1 > v2)
                    return 1;   // version1 is newer
            }

            // If all segments are equal, check the length of version strings
            if (v1Numbers.Length < v2Numbers.Length)
                return -1;
            else if (v1Numbers.Length > v2Numbers.Length)
                return 1;
            else
                return 0;   // Both versions are equal
        }
        #endregion

        #region Event Invoker
        public static IEnumerator InvokeOnState(this System.MulticastDelegate events, params object[] args)
        {
            if (events != null)
            {
                foreach (var @delegate in events.GetInvocationList())
                {
                    yield return @delegate.DynamicInvoke(args);
                }
            }
        }

        public static IEnumerator InvokeStack(this System.MulticastDelegate events, params object[] args)
        {
            if (events != null)
            {
                var delegates = events.GetInvocationList();
                var del = delegates[delegates.Length - 1];
                yield return del.DynamicInvoke(args);
            }
        }
        #endregion

        #region Async
        public static IEnumerator WaitForTask(Action unitask)
        {
            var task = Task.Run(unitask);
            yield return new WaitUntil(() => task.IsCompleted);
        }
        #endregion

        #region IEnumerable
        public static List<T> ConvertToList<T>(this T item)
        {
            var result = new List<T>();
            result.Add(item);
            return result;
        }

        public static List<Dropdown.OptionData> EnumToOptions<T>()
        {
            var options = new List<Dropdown.OptionData>();
            var names = Enum.GetNames(typeof(T));
            for (int i = 0; i < names.Length; i++)
            {
                options.Add(new Dropdown.OptionData(names[i]));
            }

            return options;
        }

        #endregion

        #region Conveter
        public static Vector3 StringToVector3(this string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static Vector3Int StringToVector3Int(this string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3Int result = new Vector3Int(
                int.Parse(sArray[0]),
                int.Parse(sArray[1]),
                int.Parse(sArray[2]));

            return result;
        }
        #endregion
    }
}
