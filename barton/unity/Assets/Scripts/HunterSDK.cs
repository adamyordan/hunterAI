using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

[System.Serializable]
public class State
{
    public bool monster_visible;
}

[System.Serializable]
public class AgentAction
{
    public string id;
}

[System.Serializable]
public class StepResponse
{
    public AgentAction action;
}

public class HunterSDK : MonoBehaviour
{
    public string Host = "http://127.0.0.1:5000/";

    public void Initiate()
    {
        StartCoroutine(PutRequest(Host + "init"));
    }

    public void Step(State state, Action<StepResponse> callback)
    {
        string stateJson = JsonUtility.ToJson(state);
        StartCoroutine(PutRequest(Host + "step", stateJson, (res) =>
        {
            StepResponse stepResponse = JsonUtility.FromJson<StepResponse>(res);
            callback(stepResponse);
        }));
    }

    public IEnumerator PutRequest(string uri, string data = "{}", Action<string> callback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, data))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                //Debug.Log("Received: " + webRequest.downloadHandler.text);
                callback?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetRequest(string uri, Action<string> callback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                //Debug.Log("Received: " + webRequest.downloadHandler.text);
                callback?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
}
