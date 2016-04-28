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
        yield return null;
    }
}
