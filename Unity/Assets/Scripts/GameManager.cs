using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
//using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	private static GameObject Player;

	private static bool gameOver = false;

	public static bool GameOver {
		get {
			return gameOver;
		}
	}

	private static DateTime StartTime = DateTime.Now;

	void Awake(){
		Player = GameObject.Find ("AirPlain");
	}

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
		Text Timetext = GameObject.Find ("Timer").GetComponent<Text> ();
		TimeSpan LimitTime = new TimeSpan (00, 10, 00);
		while (!gameOver) {
			StartCoroutine (DisplayTime (Timetext, LimitTime));
			if(Input.GetKey(KeyCode.Space)){
				StartCoroutine (GameEnd (false));
			}
			yield return null;
		}
	}

	/// <summary>
	/// 残り時間をString型に変換
	/// </summary>
	private static string TimeCastToString ()
	{
		return RestTime.Minutes.ToString ("D2") + ":" + RestTime.Seconds.ToString ("D2");//timeString;
	}

	/// <summary>
	/// GUITextに残り時間を表記する。
	/// </summary>
	/// <param name="Timetext">Timetext.</param>
	/// <param name="limitTime">Limit time.</param>
	private static IEnumerator DisplayTime (Text Timetext, TimeSpan limitTime)
	{
		TimeCalculation (limitTime);
		Timetext.text = TimeCastToString ();
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

	private static TimeSpan restTime;

	/// <summary>
	/// 時間が0を下回るとscene移行するプロパティ
	/// </summary>
	public static TimeSpan RestTime {
		set {
			restTime = value;
			if (restTime.Minutes + restTime.Seconds <= 0) {
				GameObject.Find("GameManager").GetComponent<GameManager>().StartCoroutine(GameEnd (false));
			}
		}
		get {
			return restTime;
		}
	}



	public static IEnumerator GameEnd(bool Win){
		StopGame ();
		GameManager Manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		AudioSource AudioBox =  GameObject.Find ("Main Camera").GetComponent<AudioSource> ();
		//三項演算子
//		if (Win) {
			Manager.StartCoroutine (Win ? Victory () : Defeat());
//		} else {
//			Manager.StartCoroutine(Defeat ());
//		}
		Manager.StartCoroutine (ChangeMusic(AudioBox,Win));
		yield return null;

	}

	private static IEnumerator ChangeMusic(AudioSource AudioBox,bool Win){
		int TimeSpeed = (int)(1 / Time.timeScale);
		while (AudioBox.volume > 0) {
			AudioBox.volume -= 0.05f*(Time.deltaTime*TimeSpeed);
			yield return null;
		}
		AudioBox.Stop ();
		yield return null;
		NewMusicSet (AudioBox,Win);
		StageResultText.DisplayResult (Win);
		yield return null;
	}

	//三項演算子
	private static void NewMusicSet(AudioSource AudioBox,bool Win){
//		if (Win) {
		AudioBox.clip = (AudioClip)(Resources.Load (Win ? "Sounds/FromTheNewWorld" : "Sounds/Sarabande"));
		AudioBox.volume = Win ? 0.65f : 0.5f;
//		} else {
//			AudioBox.clip = (AudioClip)(Resources.Load ("Sounds/Sarabande"));
//			AudioBox.volume = 0.5f;
//		}
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
