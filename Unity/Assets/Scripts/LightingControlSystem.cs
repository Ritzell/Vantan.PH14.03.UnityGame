using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LightingControlSystem : MonoBehaviour {
	private static List<Image> MissileIcons = new List<Image>();
	private static Queue<Image> HPBarUI = new Queue<Image>();

    private static int _missileNumber = -1;
	private static int _missileOffNumber = -1;

	// Use this for initialization
	void Start () {
		MissileIcons.Add (GameObject.Find ("MissileIconA").GetComponent<Image>());
		MissileIcons.Add (GameObject.Find ("MissileIconB").GetComponent<Image>());
		MissileIcons.Add (GameObject.Find ("MissileIconC").GetComponent<Image>());
		MissileIcons.Add (GameObject.Find ("MissileIconD").GetComponent<Image>());

		HPBarUI.Enqueue (GameObject.Find("HPBarA").GetComponent<Image>());
		HPBarUI.Enqueue (GameObject.Find("HPBarB").GetComponent<Image>());
		HPBarUI.Enqueue (GameObject.Find("HPBarC").GetComponent<Image>());
		HPBarUI.Enqueue (GameObject.Find("HPBarD").GetComponent<Image>());
		HPBarUI.Enqueue (GameObject.Find("HPBarE").GetComponent<Image>());
	}
	
	public static IEnumerator TurningOff(UIType UI){
		if (UI == UIType.Missile) {
			MissileIcons [Number (ref _missileNumber)].color = Color.gray;
		} else if (UI == UIType.HP) {
			HPBarUI.Dequeue ().color = Color.black;
			if (HPBarUI.Count == 2) {
				foreach (Image HPBar in HPBarUI) {
					HPBar.color = Color.red;
				}
				yield return null;
			}
		}
		yield return null;
	}

	public static void ShatDown(){
		while (HPBarUI.Count > 0) {
			HPBarUI.Dequeue ().color = Color.black;
		}
	}

	public static IEnumerator TurningOn(UIType UI){
		if (UI == UIType.Missile) {
			MissileIcons [Number (ref _missileOffNumber)].color = Color.white;
		}
		yield return null;
	}

	private static int Number(ref int number){
		number++;
		if(number >= MissileIcons.Count){
			number = 0;
		}
		return number;
	}
}
