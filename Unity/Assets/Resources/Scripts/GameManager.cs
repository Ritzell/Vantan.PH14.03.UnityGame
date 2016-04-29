using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// staticのため、衝突の恐れあり
/// </summary>
public class missileFactory : MonoBehaviour{
	public static GameObject newMissile;

	public static GameObject NewMissile{
		get{
			newMissile = (GameObject)Instantiate (Resources.Load("prefabs/missile"),Vector3.zero,Quaternion.identity);
			newMissile.name = newMissile.name.Substring (0,7);
			return newMissile;
		}
	}
}

public class GameManager : MonoBehaviour {
    public static bool GameOver = false;
    private static DateTime startTime = DateTime.Now;
    // Use this for initialization
    void Start () {
        StartCoroutine(Timer());
	}

	public static IEnumerator reloadMissile(Vector3 startPos, Quaternion startRot){
		float timer = 0f;
		while (timer < 3f) {
			timer += Time.deltaTime;
			yield return null;
		}
		missileFactory.NewMissile.transform.transform.parent = missile.root;
		missileFactory.newMissile.transform.localPosition = startPos;
		missileFactory.newMissile.transform.localRotation = startRot;
		yield return new WaitForSeconds(0.35f);
		Attack.missiles.Enqueue(missileFactory.newMissile);
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
