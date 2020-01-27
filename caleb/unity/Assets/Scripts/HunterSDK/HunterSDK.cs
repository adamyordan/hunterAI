using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class HunterSDK<StateType, ResponseType>
{
    MonoBehaviour mb;
    string host;

    public HunterSDK(MonoBehaviour mb, string host)
    {
        this.mb = mb;
        this.host = host;
    }

    public void Initiate()
    {
        mb.StartCoroutine(PutRequest(host + "init"));
    }

    public void Step(StateType state, Action<ResponseType> callback)
    {
        string stateJson = JsonUtility.ToJson(state);
        mb.StartCoroutine(PutRequest(host + "step", stateJson, (res) =>
        {
            ResponseType stepResponse = JsonUtility.FromJson<ResponseType>(res);
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
