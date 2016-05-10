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
		missileFactory script = GameObject.Find ("GameManager").GetComponent<missileFactory> ();
		yield return new WaitForSeconds (3);
		script.NewMissile.transform.transform.parent = missile.root;
		script.newMissile.transform.localPosition = startPos;
		script.newMissile.transform.localRotation = startRot;
		yield return new WaitForSeconds(0.1f);
		Attack.missiles.Enqueue(script.newMissile);
		yield return null;

	}

	private TimeSpan time;

	public IEnumerator Timer()
    {
		GUIText Timetext = GameObject.Find("Timer").GetComponent<GUIText>();
		TimeSpan elapsedTime;
        TimeSpan limitTime = new TimeSpan(00, 10, 00);
        while (!GameOver)
        {
			elapsedTime = (TimeSpan)(DateTime.Now - startTime);
			Time = limitTime - elapsedTime;
			Timetext.text = (time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2"));
            yield return null;
        }
    }

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
		//yield return new WaitForSeconds (3);
		SceneManager.LoadScene ("Result");
		//Application.LoadLevel("Result");
		//yield return null;
	}
}
