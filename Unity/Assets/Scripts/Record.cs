using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Record : MonoBehaviour {
	private static Text BattleIssue;
	private static Text TimeRecord;
	private static Text MissileCount;

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
	}
	
	// Update is called once per frame
	void Start () {
		TextEntry ();
	}

	private static void TextEntry(){
		BattleIssue.text = isVictory ? "Victory" : "Defeat";
		BattleIssue.color = isVictory ? Color.red : Color.blue;
		TimeRecord.text = "プレイ時間 : " + GameManager.TimeCastToString (new System.TimeSpan(0,10,0) - GameManager.RestTime);
		MissileCount.text = "発射ミサイル数　: " + GameManager.MissileCounter.ToString ();
	}
}
