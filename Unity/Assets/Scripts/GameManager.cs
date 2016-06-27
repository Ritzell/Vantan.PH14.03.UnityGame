using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
//using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour
{

	private static CameraSystem CameraS;
	private static DateTime StartTime;
	private static GameManager Manager;
	private static MissileFactory Factory;


	private static bool gameOver = false;
	public static bool GameOver {
		set{
			gameOver = value;
		}
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

	private static int _enemyMissiles = 0;
	public static int EnemyMissiles {
		set {
			_enemyMissiles += value;
		}get {
			return _enemyMissiles;
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
				Manager.StartCoroutine(GameEnd (false));
			}
		}
		get {
			return restTime;
		}
	}


	void Awake(){
		DontDestroyOnLoad (GameObject.Find("GameManager"));
		CameraS = FindObjectOfType<CameraSystem>();
	}

	public void StartStage ()
	{
		QualitySettings.vSyncCount = 0; // VSyncをOFFにする
		QualitySettings.antiAliasing = 0;
		Camera.main.renderingPath = RenderingPath.Forward;
		CameraS = FindObjectOfType<CameraSystem>();
		Manager = GameObject.FindObjectOfType<GameManager>();
		Factory = GameObject.FindObjectOfType<MissileFactory>();
		StartTime = DateTime.Now;
//		CameraSetting.StartUp ();
		StartCoroutine (Timer ());//タイマーを起動
	}

	public IEnumerator ReloadMissile (Vector3 StartPos, Quaternion StartRot)
	{
		yield return new WaitForSeconds (3f);
		Attack.PlayerMissiles.Enqueue (Factory.NewPlayerMissile (StartPos, StartRot,true));
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

	public static IEnumerator FlashLoadScene(string SceneName){
		bool isOut = false;
		CameraS.StartCoroutine(CameraS.Flash(3f,true,1,GameObject.Find("Canvas"),fadeout => isOut = fadeout));
		while (!isOut) {
			yield return null;
		}
		SceneManager.LoadSceneAsync (SceneName);
		yield return null;
	}

	public static IEnumerator GameEnd(bool isWin){
		StopGame ();
		AudioSource AudioBox =  Manager.GetComponent<AudioSource> ();
		Record.IsVictory = isWin;
		Manager.StartCoroutine (Manager.StopSounds());
		Manager.StartCoroutine (isWin ? Victory () : Defeat());
		Manager.StartCoroutine (ChangeMusic(AudioBox,isWin));
		yield return null;

	}

	public IEnumerator StopSounds(){
		foreach (AudioSource audio in FindObjectsOfType<AudioSource>()) {
			audio.Stop ();
		}
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
		yield return new WaitForSeconds (5*Time.timeScale);
		while (true) {
			if (isNext()) {
				yield return Manager.StartCoroutine(FlashLoadScene ("Result"));
				yield break;
			}
			yield return null;
		}

	}

	public static IEnumerator Defeat ()
	{
		CameraS.StartCoroutine (CameraS.CameraOut ());
		yield return new WaitForSeconds (5*Time.timeScale);
		while (true) {
			if (isNext()) {
				FindObjectOfType<CameraSystem> ().StopCoroutine ("CameraOut");
				yield return Manager.StartCoroutine(FlashLoadScene ("Result"));
				yield break;
			}
			yield return null;
		}
	}

	private static bool isNext(){
		return Input.GetKeyDown (KeyCode.Space) || Input.GetKey (KeyCode.JoystickButton9);
	}

	private static void StopGame(){
		gameOver = true;
		Time.timeScale = 0.015f;
	}
}