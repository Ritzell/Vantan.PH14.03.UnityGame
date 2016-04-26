using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {
	public static Queue<GameObject> missiles = new Queue<GameObject> ();
	//public GameObject[] missiles = new GameObject[4];
	private float reloading = 0.5f;
	// Use this for initialization
	void Start () {
		missiles.Enqueue (GameObject.Find ("missileA"));
		missiles.Enqueue (GameObject.Find ("missileB"));
		missiles.Enqueue (GameObject.Find ("missileC"));
		missiles.Enqueue (GameObject.Find ("missileD"));

	}

	// Update is called once per frame
	void Update () {
		reloading += Time.deltaTime;
		if (reloading > 1f) {
			if ((Input.GetAxis ("RTrigger") == 1 || Input.GetKeyDown (KeyCode.C))) {
				StartCoroutine (missiles.Dequeue().GetComponent<missile> ().straight ());
				reloading = 0f;
			}
//			if (Input.GetAxis ("LTrigger") == 1) {
//				missiles [mN].GetComponent<Rigidbody> ().AddForce (transform.forward * 2500f);
//			}
		}
	}
}
