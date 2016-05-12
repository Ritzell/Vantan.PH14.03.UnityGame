using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gun : MonoBehaviour {
	private  static List<GameObject> muzzles = new List<GameObject>();
	private static bulletFactory factory;
	// Use this for initialization

	void Start(){
		factory = GameObject.Find ("GameManager").GetComponent<bulletFactory>();
		muzzles.Add (GameObject.Find ("muzzleA"));
		muzzles.Add (GameObject.Find ("muzzleB"));
	}

	public IEnumerator shoot(){
		foreach (GameObject ob in muzzles) {
			Muzzle script = ob.GetComponent<Muzzle>();
			StartCoroutine (script.Ignition());
			StartCoroutine(factory.newBullet(ob.transform,ob.transform.position,ob.transform.rotation));
			yield return null;
		}
		yield return null;
	}
}
