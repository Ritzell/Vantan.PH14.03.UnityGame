using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MissileUI : MonoBehaviour {
	private static List<Image> MissileIcons = new List<Image>(); 

	// Use this for initialization
	void Awake () {
		MissileIcons.Add (GameObject.Find ("MissileIconA").GetComponent<Image>());
		MissileIcons.Add (GameObject.Find ("MissileIconB").GetComponent<Image>());
		MissileIcons.Add (GameObject.Find ("MissileIconC").GetComponent<Image>());
		MissileIcons.Add (GameObject.Find ("MissileIconD").GetComponent<Image>());
	}
	
	public static IEnumerator TurningOff(int Number){
		MissileIcons[Number].color = Color.gray;
		Debug.Log (Number);
		yield return null;
	}
	public static IEnumerator TurningOn(int Number){
		MissileIcons[Number].color = Color.white;
		yield return null;
	}
}
