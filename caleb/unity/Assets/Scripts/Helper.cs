using UnityEngine;
using System;
using System.Collections;

public class Helper : MonoBehaviour
{
    public static void RunLater(MonoBehaviour mb, Action action, float delayTime)
    {
        mb.StartCoroutine(RunLaterCoroutine(action, delayTime));
    }

    static IEnumerator RunLaterCoroutine(Action action, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        action();
    }

    public static Vector2Int GridVector(Vector3 vector)
    {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.z));
    }

    public static Vector2 FlatVector(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static string StringifyGrid(Vector2Int vector)
    {
        return string.Format("{0},{1}", vector.x, vector.y);
    }

    public static Vector2Int ParseGrid(string str)
    {
        string[] splitted = str.Split(',');
        int x = int.Parse(splitted[0]);
        int y = int.Parse(splitted[1]);
        return new Vector2Int(x, y);
    }
}
