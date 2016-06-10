using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Record : MonoBehaviour {
	private static Text BattleIssue;
	private static Text TimeRecord;
	private static Text MissileCount;
	private static Text Rank;
	private static TimeSpan ElapsedTime;

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
	}

	private IEnumerator NextScene(){
		while(true){
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene ("title");
				GameObject.Find ("GameManager").GetComponent<AudioSource> ().Stop ();
			}
			yield return null;
		}
	}

	private static void ResultEntry(){
		BattleIssue.text = isVictory ? VictoryEntry() : DefeatEntry();
		TimeRecord.text = "プレイ時間 : " + GameManager.TimeCastToString(ElapsedTime);
		MissileCount.text = "発射ミサイル数　: " + GameManager.MissileCounter.ToString ();
	}

	private static string VictoryEntry(){
		BattleIssue.color = Color.red;
		Rank.text = EvaluationRank (true);
		return "Victory";
	}

	private static string DefeatEntry(){
		BattleIssue.color = Color.blue;
		Rank.text = EvaluationRank (false);
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
