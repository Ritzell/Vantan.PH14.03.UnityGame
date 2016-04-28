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

	public static IEnumerator reloadMissile(string ObName, Vector3 startPos, Quaternion startRot){
		float timer = 0f;
		while (timer < 3f) {
			timer += Time.deltaTime;
			yield return null;
		}
		GameObject newMissile = (GameObject)Instantiate (Resources.Load("prefabs/"+ObName),Vector3.zero,Quaternion.identity);
		newMissile.transform.parent = missile.root;
		newMissile.name = ObName.Substring (0,8);
		newMissile.transform.localPosition = startPos;
		newMissile.transform.localRotation = startRot;
		yield return new WaitForSeconds(0.35f);
		Attack.missiles.Enqueue(newMissile);
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
