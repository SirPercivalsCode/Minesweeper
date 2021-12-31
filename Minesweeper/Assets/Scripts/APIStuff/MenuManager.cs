using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MenuManager : MonoBehaviour
{
    public Text inputField;
    public Text usernameText;

    public GameObject changeNamePanel;
    public GameObject inputNamePanel;

    public string username;

    public string receivedToken;

    public class CreateUser
    {
        public string username;
    }

    public class ChangeUser
    {
        public string username;
        public string key;
    }

    void Start()
    {
        try
        {
            string uuid = File.ReadAllText(Application.dataPath + "/uuid.txt");
            StartCoroutine(GetText(uuid));

            changeNamePanel.SetActive(true);
            inputNamePanel.SetActive(false);
        }
        catch
        {
            changeNamePanel.SetActive(false);
            inputNamePanel.SetActive(true);

            username = null;
        }
    }

    public void StartGame()
    {
        if (inputField.text != "")
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void SaveUsername()
    {
        if(username != null)
        {
            NewName();
            return;
        }

        CreateUser newUser = new CreateUser();
        newUser.username = inputField.text;

        string userjson = JsonUtility.ToJson(newUser);

        if (inputField.text != "")
        {
            username = inputField.text;

            StartCoroutine(PostRequest("https://rqxhdb.deta.dev/user/add", userjson));

            usernameText.text = username;

            changeNamePanel.SetActive(true);
            inputNamePanel.SetActive(false);
        }
    }

    private void NewName()
    {
        ChangeUser user = new ChangeUser();
        user.username = inputField.text;
        user.key = File.ReadAllText(Application.dataPath + "/uuid.txt");

        string userjson = JsonUtility.ToJson(user);

        if (inputField.text != "")
        {
            username = inputField.text;

            StartCoroutine(PostRequest("https://rqxhdb.deta.dev/user/change", userjson));

            usernameText.text = username;

            changeNamePanel.SetActive(true);
            inputNamePanel.SetActive(false);
        }
    }

    public void ChangeUsername()
    {
        changeNamePanel.SetActive(false);
        inputNamePanel.SetActive(true);
    }

    IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            yield break;
        }
        else
        {
            try
            {
                string uuid = File.ReadAllText(Application.dataPath + "/uuid.txt");

                yield break;
            }
            catch
            {
                receivedToken = uwr.downloadHandler.text;
                Debug.Log("Received token: " + receivedToken);
                File.WriteAllText(Application.dataPath + "/uuid.txt", receivedToken);
            }
        }
    }

    IEnumerator GetText(string _uuid)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://rqxhdb.deta.dev/user/get?key=" + _uuid);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            username = www.downloadHandler.text;
            Debug.Log(www.downloadHandler.text);

            usernameText.text = username;
        }
    }
}
