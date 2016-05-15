using UnityEngine;
using System.Collections;
using System;

//using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	
	private static bool gameOver = false;

	public static bool GameOver {
		get {
			return gameOver;
		}
	}

	private static DateTime StartTime = DateTime.Now;
	// Use this for initialization

	void Start ()
	{
		QualitySettings.vSyncCount = 0; // VSyncをOFFにする
		Application.targetFrameRate = 60; // ターゲットフレームレートを60に設定
		StartCoroutine (Timer ());
	}

	public IEnumerator ReloadMissile (Vector3 StartPos, Quaternion StartRot)
	{
		MissileFactory Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
		yield return new WaitForSeconds (3.1f);
		Attack.Missiles.Enqueue (Factory.NewMissile (StartPos, StartRot));
		yield return null;
	}

	/// <summary>
	/// 制限時間の設定と残り時間を計算するメソッドの実行
	/// </summary>
	private IEnumerator Timer ()
	{
		GUIText Timetext = GameObject.Find ("Timer").GetComponent<GUIText> ();
		TimeSpan LimitTime = new TimeSpan (00, 10, 00);
		while (!gameOver) {
			StartCoroutine (displayTime (Timetext, LimitTime));
			yield return null;
		}
	}

	/// <summary>
	/// 残り時間をString型に変換
	/// </summary>
	private string TimeCastToString ()
	{
		return Time.Minutes.ToString ("D2") + ":" + Time.Seconds.ToString ("D2");//timeString;
	}

	/// <summary>
	/// 残り時間を計算
	/// </summary>
	private void TimeCalculation (TimeSpan limitTime)
	{
		TimeSpan elapsedTime = (TimeSpan)(DateTime.Now - StartTime);
		Time = limitTime - elapsedTime;
	}

	/// <summary>
	/// GUITextに残り時間を表記する。
	/// </summary>
	/// <param name="Timetext">Timetext.</param>
	/// <param name="limitTime">Limit time.</param>
	private IEnumerator displayTime (GUIText Timetext, TimeSpan limitTime)
	{
		TimeCalculation (limitTime);
		Timetext.text = TimeCastToString ();
		yield return null;
	}


	private TimeSpan RestTime;

	/// <summary>
	/// 時間が0を下回るとscene移行するプロパティ
	/// </summary>
	private TimeSpan Time {
		set {
			RestTime = value;
			if (RestTime.Minutes + RestTime.Seconds <= 0) {
				GameManager.loadScene ();
			}
		}
		get {
			return RestTime;
		}
	}

	public static void loadScene ()
	{
		SceneManager.LoadScene ("Result");
	}
}
