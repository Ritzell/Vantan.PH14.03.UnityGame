using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static bool GameOver = false;
    private static DateTime startTime = DateTime.Now;
    // Use this for initialization
    void Start () {
        StartCoroutine(Timer());
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
	
	public IEnumerator Timer()
    {
        Text Timetext = GameObject.Find("Timer").GetComponent<Text>();
        TimeSpan time;
        TimeSpan limitTime = new TimeSpan(00, 10, 00);
        while (!GameOver)
        {
            time = (TimeSpan)(DateTime.Now - startTime);
            time = limitTime - time;
            Timetext.text = (time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2"));
            yield return null;
        }
    }
}
