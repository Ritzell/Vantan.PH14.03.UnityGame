using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	private float speed = 80.0f;
	public	GameObject	body;
	public	GameObject 	myCamera;
	public	GameObject	look;
	engineSound engineS;
	// Use this for initialization
	void Start () {
		engineS = GameObject.Find("engine").GetComponent<engineSound> ();
	}
	
	// Update is called once per frame
	void Update () {
		float mY =  Input.GetAxis ("Horizontal");
		float mX = Input.GetAxis ("Vertical");

		//myCamera.transform.LookAt(look.transform);
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
		transform.Rotate (mX/2, 0f, mY*3);

		if (Input.GetKey(KeyCode.Joystick1Button13)) {
			Speed= -1f;
		}

		if (Input.GetKey(KeyCode.Joystick1Button14)) {
			Debug.Log ("speedUP");
			Speed=+1f;
		}
	}
	public float Speed {
		set{
			if (this.speed >= 60f && this.speed <= 280f) {
				this.speed += value;
			}
			if (this.speed < 60) {
				this.speed = 60f;
			} else if (this.speed > 280) {
				this.speed = 280f;
			}
			engineS.Pitch = this.speed;
		}get{
			if (this.speed < 60) {
				return 60f;
			}
			return this.speed;
		}
	}
}
