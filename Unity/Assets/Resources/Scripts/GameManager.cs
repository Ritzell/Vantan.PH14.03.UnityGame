using UnityEngine;
using System.Collections;
using System;
//using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static bool GameOver = false;
    private static DateTime startTime = DateTime.Now;
    // Use this for initialization

    void Start () {
		StartCoroutine (Timer ());
	}

	public IEnumerator reloadMissile(Vector3 startPos, Quaternion startRot){
		MissileFactory script = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
		yield return new WaitForSeconds (3);
		script.NewMissile.transform.transform.parent = GameObject.Find ("missiles").transform;
		script.newMissile.transform.localPosition = startPos;
		script.newMissile.transform.localRotation = startRot;
		yield return new WaitForSeconds(0.1f);
		Attack.missiles.Enqueue(script.newMissile);
		yield return null;
	}

	/// <summary>
	/// 制限時間の設定と残り時間を計算するメソッドの実行
	/// </summary>
	public IEnumerator Timer()
    {
		GUIText Timetext = GameObject.Find("Timer").GetComponent<GUIText>();
        TimeSpan limitTime = new TimeSpan(00, 10, 00);
        while (!GameOver)
        {
			StartCoroutine(displayTime (Timetext, limitTime));
            yield return null;
        }
    }

	/// <summary>
	/// 残り時間をString型に変換
	/// </summary>
	public string timeCastToString(){
		return Time.Minutes.ToString ("D2") + ":" + Time.Seconds.ToString ("D2");//timeString;
	}

	/// <summary>
	/// 残り時間を計算
	/// </summary>
	public void timeCalculation(TimeSpan limitTime){
		TimeSpan elapsedTime = (TimeSpan)(DateTime.Now - startTime);
		Time = limitTime - elapsedTime;
	}

	/// <summary>
	/// GUITextに残り時間を表記する。
	/// </summary>
	/// <param name="Timetext">Timetext.</param>
	/// <param name="limitTime">Limit time.</param>
	public IEnumerator displayTime(GUIText Timetext, TimeSpan limitTime){
		timeCalculation (limitTime);
		Timetext.text = timeCastToString();
		yield return null;
	}


	private TimeSpan time;

	/// <summary>
	/// 時間が0を下回るとscene移行するプロパティ
	/// </summary>
	public TimeSpan Time{
		set{
			time = value;
			if (time.Minutes + time.Seconds <= 0) {
				GameManager.loadScene ();
			}
		}get{
			return time;
		}
	}

	public static void loadScene(){
		SceneManager.LoadScene ("Result");
	}
}
