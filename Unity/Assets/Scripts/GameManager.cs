using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
//using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	
	private static DateTime StartTime = DateTime.Now;
	private static GameManager Manager;

	private static bool gameOver = false;
	public static bool GameOver {
		get {
			return gameOver;
		}
	}

	private static int MissileCount;
	public static int MissileCounter {
		set {
			MissileCount += value;
		}get {
			return MissileCount;
		}
	}

	private static TimeSpan restTime;

	/// <summary>
	/// 時間が0を下回るとscene移行するプロパティ
	/// </summary>
	public static TimeSpan RestTime {
		set {
			restTime = value;
			if (restTime.Minutes + restTime.Seconds <= 0) {
				Manager.GetComponent<GameManager>().StartCoroutine(GameEnd (false));
			}
		}
		get {
			return restTime;
		}
	}


	void Awake(){
		DontDestroyOnLoad (GameObject.Find("GameManager"));
		Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

	}

	void Start ()
	{
		QualitySettings.vSyncCount = 0; // VSyncをOFFにする
		Application.targetFrameRate = 60; // ターゲットフレームレートを60に設定
		StartCoroutine (Timer ());
		if (SystemInfo.supportsVibration) {
			Debug.Log ("振動対応");
			//Handheld.Vibrate ();
		} else {
			Debug.Log ("振動非対応");
		}
	}

	public IEnumerator ReloadMissile (Vector3 StartPos, Quaternion StartRot)
	{
		MissileFactory Factory = Manager.GetComponent<MissileFactory> ();
		yield return new WaitForSeconds (3f);
		Attack.Missiles.Enqueue (Factory.NewMissile (StartPos, StartRot));
		yield return null;
	}

	/// <summary>
	/// 制限時間の設定と残り時間を計算するメソッドの実行
	/// </summary>
	private IEnumerator Timer ()
	{
		Text Timetext = GameObject.Find ("Timer").GetComponent<Text> ();
		TimeSpan LimitTime = new TimeSpan (00, 10, 00);
		while (!gameOver) {
			StartCoroutine (DisplayTime (Timetext, LimitTime));
			yield return null;
		}
	}

	/// <summary>
	/// 残り時間をString型に変換
	/// </summary>
	public static string TimeCastToString (TimeSpan Time)
	{
		return Time.Minutes.ToString ("D2") + ":" + Time.Seconds.ToString ("D2");//timeString;
	}

	/// <summary>
	/// GUITextに残り時間を表記する。
	/// </summary>
	/// <param name="Timetext">Timetext.</param>
	/// <param name="limitTime">Limit time.</param>
	private static IEnumerator DisplayTime (Text Timetext, TimeSpan limitTime)
	{
		TimeCalculation (limitTime);
		Timetext.text = TimeCastToString (RestTime);
		yield return null;
	}

	/// <summary>
	/// 残り時間を計算
	/// </summary>
	private static void TimeCalculation (TimeSpan limitTime)
	{
		TimeSpan elapsedTime = (TimeSpan)(DateTime.Now - StartTime);
		RestTime = limitTime - elapsedTime;
	}



	public static IEnumerator GameEnd(bool isWin){
		StopGame ();
//		isWin = true;
		AudioSource AudioBox =  Manager.GetComponent<AudioSource> ();
		Record.IsVictory = isWin;
		Manager.StartCoroutine (isWin ? Victory () : Defeat());
		Manager.StartCoroutine (ChangeMusic(AudioBox,isWin));
		yield return null;

	}

	private static IEnumerator ChangeMusic(AudioSource AudioBox,bool isWin){
		int TimeSpeed = (int)(1 / Time.timeScale);
		while (AudioBox.volume > 0) {
			AudioBox.volume -= 0.05f*(Time.deltaTime*TimeSpeed);
			yield return null;
		}
		AudioBox.Stop ();
		yield return null;
		NewMusicSet (AudioBox,isWin);
		StageResultText.DisplayResult (isWin);
		yield return null;
	}

	private static void NewMusicSet(AudioSource AudioBox,bool isWin){
		AudioBox.clip = (AudioClip)(Resources.Load (isWin ? "Sounds/FromTheNewWorld" : "Sounds/Sarabande"));
		AudioBox.volume = isWin ? 0.65f : 0.5f;
		AudioBox.loop = false;
		AudioBox.Play ();
	}

	public static IEnumerator Victory(){
		while (true) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene ("Result");
				yield return null;
			}
			yield return null;
		}

	}

	public static IEnumerator Defeat ()
	{
		while (true) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene ("Result");
				yield return null;
			}
			yield return null;
		}
	}

	private static void StopGame(){
		gameOver = true;
		Time.timeScale = 0.015f;
	}
}
