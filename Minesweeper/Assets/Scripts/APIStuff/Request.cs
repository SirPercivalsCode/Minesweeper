using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Request : MonoBehaviour
{
    /*
    For any HTTP transaction, the normal code flow is:
    1. Create a Web Request object.
    2. Configure the Web Request object. ...
    3. (Optional) Create an Upload Handler and attach it to the Web Request. ...
    4. (Optional) Create a Download Handler and attach it to the Web Request.
    5. Send the Web Request.
    */

    enum RequestType{
        GET, POST
    }


    public virtual IEnumerator SimpleGetRequest(string getURL)
    {
        UnityWebRequest www = UnityWebRequest.Get(getURL);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            yield return www.downloadHandler.text;
        }
    }


    //Copy Paste Code https://youtu.be/K9uVHI645Pk
    /*
    private IEnumerator MakeRequests() {
        // GET
        var getRequest = CreateRequest("https://jsonplaceholder.typicode.com/todos/1");
        yield return getRequest.SendWebRequest();
        var deserializedGetData = JsonUtility.FromJson<Todo>(getRequest.downloadHandler.text);

        // POST
        var dataToPost = new PostData(){Hero = "John Wick", PowerLevel = 9001};
        var postRequest = CreateRequest("https://reqbin.com/echo/post/json", RequestType.POST, dataToPost);
        yield return postRequest.SendWebRequest();
        var deserializedPostData = JsonUtility.FromJson<PostResult>(postRequest.downloadHandler.text);

        // Trigger continuation of game flow
    }
    */


    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null) {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null) {
            //var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            //request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private void AttachHeader(UnityWebRequest request,string key,string value)
    {
        request.SetRequestHeader(key, value);
    }
}
