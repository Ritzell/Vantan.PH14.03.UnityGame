using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour {

	private static Queue<GameObject> HPBarUI = new Queue<GameObject>();

	// Use this for initialization
	void Awake () {
		HPBarUI.Enqueue (GameObject.Find("HPBarA"));
		HPBarUI.Enqueue (GameObject.Find("HPBarB"));
		HPBarUI.Enqueue (GameObject.Find("HPBarC"));
		HPBarUI.Enqueue (GameObject.Find("HPBarD"));
		HPBarUI.Enqueue (GameObject.Find("HPBarE"));
	}

	public static IEnumerator LifeExhaustion(){
		HPBarUI.Dequeue ().SetActive(false);
		yield return null;
	}
}
