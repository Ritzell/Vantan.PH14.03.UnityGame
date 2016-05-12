using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {
	private  static List<GameObject> Muzzules = new List<GameObject>();
	private static BulletFactory Factory;
	// Use this for initialization

	void Start(){
		Factory = GameObject.Find ("GameManager").GetComponent<BulletFactory>();
		Muzzules.Add (GameObject.Find ("muzzleA"));
		Muzzules.Add (GameObject.Find ("muzzleB"));
	}

	public IEnumerator shoot(){
		foreach (GameObject ob in Muzzules) {
			Muzzle script = ob.GetComponent<Muzzle>();
			StartCoroutine (script.Ignition());
			StartCoroutine(Factory.MakeBullet(ob.transform,ob.transform.position,ob.transform.rotation));
			yield return null;
		}
		yield return null;
	}
}
