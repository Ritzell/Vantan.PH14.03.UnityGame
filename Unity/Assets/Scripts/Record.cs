using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class Record : MonoBehaviour {
	private static Text BattleIssue;
	private static Text TimeRecord;
	private static Text MissileCount;
	private static Text Rank;
	private static TimeSpan ElapsedTime;


	[SerializeField]
	private RenderTexture target;
	[SerializeField]
	private Image BackGroundImage;

	private static bool isVictory;
	public static bool IsVictory{
		set{
			isVictory = value;
		}
	}

	// Use this for initialization
	void Awake () {
		BattleIssue = GameObject.Find ("BattleIssue").GetComponent<Text>();
		TimeRecord = GameObject.Find ("TimeRecord").GetComponent<Text>();
		MissileCount = GameObject.Find ("MissileCount").GetComponent<Text>();
		Rank = GameObject.Find ("RankText").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Start () {
		Time.timeScale = 1;
		ElapsedTime =  new TimeSpan (0, 10, 0) - GameManager.RestTime;
		StartCoroutine (NextScene ());
		ResultEntry ();
		FindObjectOfType<CameraSetting> ().OnScene (Scenes.Result);
	}

	[SerializeField]
	private Sprite DefeatImage;

	private void ChangeImage(){
		if (File.Exists (ImageCamera.ImagePath)) {
			byte[] bytes = File.ReadAllBytes (ImageCamera.ImagePath);
			Texture2D ResultTexture = new Texture2D (Screen.width, Screen.height);
			ResultTexture.LoadImage (bytes);
			BackGroundImage.sprite = Sprite.Create (ResultTexture, new Rect (0, 0, Screen.width, Screen.height), Vector2.zero);
			return;
		} else {
			Debug.Log ("a");
			//画像がなければ画像を変えない
			return;
		}
	}

	private IEnumerator NextScene(){
		while(true){
			if (Input.GetKeyDown (KeyCode.Space) || Input.GetKey (KeyCode.JoystickButton9)) {
				Destroy(GameObject.Find("Main Camera"));
				SceneManager.LoadScene ("title");
				GameObject.Find ("GameManager").GetComponent<AudioSource> ().Stop ();
				if (File.Exists(ImageCamera.ImagePath))
				{
					File.Delete(ImageCamera.ImagePath) ;
				}
				Destroy (GameObject.Find ("GameManager"));
			}
			yield return null;
		}
	}

	private static void ResultEntry(){
		BattleIssue.text = isVictory ? VictoryEntry() : DefeatEntry();
		TimeRecord.text = "プレイ時間 : " + GameManager.TimeCastToString(ElapsedTime);
		FindObjectOfType<Record>().StartCoroutine (MissileCounter (GameManager.MissileCounter));
	}

	private static IEnumerator MissileCounter(int Count){
		float UpCounter = 0;
		while (UpCounter < Count) {
			UpCounter += Count * (2.5f*Time.deltaTime);
			MissileCount.text = "発射ミサイル数　: " + (int)UpCounter;
			yield return null;
		}
		MissileCount.text = "発射ミサイル数　: " + Count;
		yield return null;
	}

	private static string VictoryEntry(){
		BattleIssue.color = Color.red;
		Rank.text = EvaluationRank (true);
		return "Victory";
	}

	private static string DefeatEntry(){
		BattleIssue.color = Color.blue;
		Rank.text = EvaluationRank (false);
		FindObjectOfType<Record> ().ChangeImage ();
		return "Defeat";
	}

	private static string EvaluationRank(bool isWin){
		int ElapsedMinutes = ElapsedTime.Minutes;
		int ElapsedSecond = ElapsedTime.Seconds;
		if (isWin) {
			if (ElapsedMinutes == 0 && ElapsedSecond <= 30) {
				return "Cheat";
			} else if (ElapsedMinutes <= 2 && ElapsedSecond <= 30) {
				return "Congratulation";
			} else if (ElapsedMinutes <= 4) {
				return "Excellent";
			} else if (ElapsedMinutes <= 6) {
				return "Great";
			} else if (ElapsedMinutes <= 9) {
				return "Good";
			}
		} else {
			if (ElapsedMinutes == 0 && ElapsedSecond <= 30) {
				return "Noob";
			} else if (ElapsedMinutes <= 2 && ElapsedSecond <= 30) {
				return "awful";
			} else if (ElapsedMinutes <= 4) {
				return "Inferior";
			} else if (ElapsedMinutes <= 6) {
				return "Beginner";
			} else if (ElapsedMinutes <= 9) {
				return "No Good";
			}
		}
		return "No Comment";
	}
}