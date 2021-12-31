using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
	public static int highscore;
	public int score;
	public Text scoreDisplay;
	public Text scoreDisplay2;
	public Text highscoreDisplay;

	//public GameObject pipeMan;

	public float jumpforce;
	private Rigidbody2D rb;

	private bool isColliding;
	private bool gameOver;

	public Animator transition;


	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		string uuid = File.ReadAllText(Application.dataPath + "/uuid.txt");

		StartCoroutine(GetText(uuid));
	}

	void Update()
	{
		isColliding = false;
		scoreDisplay.GetComponent<Text>().text = score.ToString();
		scoreDisplay2.GetComponent<Text>().text = score.ToString();
		
		if (Input.GetKeyDown(KeyCode.Space) && gameOver == false) {
			rb.velocity = Vector2.up * jumpforce;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (isColliding) return;
		isColliding = true;
		if (other.CompareTag("Ceiling")) {
			return;
		}
		if (other.CompareTag("Scorepoint")) {
			score += 1;
		} else {
			//Game Over
			gameOver = true;

			scoreDisplay2.text = score.ToString();

			if (score > highscore){
				highscore = score;

				UpdateHighscore();

				highscoreDisplay.text = highscore.ToString();
			}
			
			transition.SetTrigger("Start");
			//pipeMan.GetComponent<PipeManager>().speed = 0f;
			//pipeMan.GetComponent<PipeManager>().accel = 0f;
		}
	}

	/*
	public void LoadLevel()
	{
		transform.position = new Vector2(-4.5f, 0f);
		score = 0;
		transition.SetTrigger("End");
		pipeMan.GetComponent<PipeManager>().speed = pipeMan.GetComponent<PipeManager>().startSpeed;
		pipeMan.GetComponent<PipeManager>().accel = pipeMan.GetComponent<PipeManager>().startAccel;

		pipeMan.GetComponent<PipeManager>().pipe1.transform.position = new Vector2(10f, 0f);
		pipeMan.GetComponent<PipeManager>().pipe2.transform.position = new Vector2(20f, -1.5f);
		pipeMan.GetComponent<PipeManager>().pipe3.transform.position = new Vector2(30f, 1.8f);
		pipeMan.GetComponent<PipeManager>().pipe4.transform.position = new Vector2(40f, -1.2f);
		gameOver = false;
	}
	*/

	public class Score
	{
		public string key;
		public int newhigh;
	}

	private void UpdateHighscore()
    {
		string uuid = File.ReadAllText(Application.dataPath + "/uuid.txt");

		/*
		Post request
		POST https://rqxhdb.deta.dev/leaderboard/set
		key: str
		newhigh: int
		*/

		Score newScore = new Score();
		newScore.key = uuid;
		newScore.newhigh = score;

		string scorejson = JsonUtility.ToJson(newScore);

		StartCoroutine(PostRequest("https://rqxhdb.deta.dev/leaderboard/set", scorejson));
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
			//yield return new WaitForSecondsRealtime(4);
		}
	}
	
	IEnumerator GetText(string _uuid)
	{
		UnityWebRequest www = UnityWebRequest.Get("https://rqxhdb.deta.dev/leaderboard/get?key=" + _uuid);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.error);
		}
		else
		{
			// Show results as text
			int.TryParse(www.downloadHandler.text, out highscore);
			Debug.Log("Highscore: " + www.downloadHandler.text);

			highscoreDisplay.text = highscore.ToString();
		}
	}

	IEnumerator GetLeaderboardOverall(string _uuid)
	{
		UnityWebRequest www = UnityWebRequest.Get("https://rqxhdb.deta.dev/leaderboard/get");
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.error);
		}
		else
		{
			// Show results as text
			
		}
	}
}
