using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour {
	public GameObject[] missiles = new GameObject[4];
	private int mn = -1;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Joystick1Button7)||Input.GetKeyDown(KeyCode.C)) {
			StartCoroutine(missiles[mN].GetComponent<missile>().straight());
		}
		if (Input.GetKey(KeyCode.Joystick1Button6)) {
			missiles[mN].GetComponent<Rigidbody>().AddForce(transform.forward * 2500f);
		}
	}

	public int mN{
		set{
				mn += value;
		}get{
			if (mn > 2) {
				mn = -1;
			}
			mn++;
			return mn;
		}
	}
}
