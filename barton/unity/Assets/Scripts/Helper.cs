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
}
