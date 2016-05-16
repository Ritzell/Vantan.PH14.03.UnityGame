using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missile : MonoBehaviour
{
	[SerializeField]
	private AudioClip AudioClip1;
	[SerializeField]
	private AudioClip AudioClip2;

	private AudioSource AudioS;

	private float Speed;
//時速3000km

	private static Airframe AirFrame;

	private Vector3 StartPos;
	private Quaternion StartRot;


	void Awake(){
		AirFrame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		AudioS = gameObject.GetComponent<AudioSource> ();
	}

	void Start ()
	{
		
		AudioS.clip = AudioClip1;
		StartPos = transform.localPosition;
		StartRot = transform.localRotation;
		Speed = Random.Range (600, 690);
	}

	public IEnumerator Straight ()
	{
		ShootReady ();
		while (!GameManager.GameOver) {
			try{
			StartCoroutine (MoveForward ());
			}catch{
			}
			yield return null;
		}
	}



	public IEnumerator Straight (Transform tgt)
	{
		shootReady_E ();
		transform.LookAt (tgt);
		while (!GameManager.GameOver) {
			StartCoroutine (MoveForward ());
			yield return null;
		}
	}

	public IEnumerator Tracking (Transform tgt)
	{
		ShootReady ();
		while (!GameManager.GameOver) {
			try{
			StartCoroutine (GetAiming (tgt, true));
			StartCoroutine (MoveForward ());
			}catch{
			}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator Tracking_E (Transform tgt)
	{
		shootReady_E ();
		float delay = 0f;
		while (!GameManager.GameOver) {
			delay += Time.deltaTime;
			try{
			if (delay >= 0.4f) {
				StartCoroutine (GetAiming (tgt, false));
				delay = 0f;
				}
			StartCoroutine (MoveForward ());
			}catch{
			}
			yield return null;
		}
		yield return null;
	}

	private void ShootReady ()
	{
		AirFrame.Reload (StartPos, StartRot); //StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
		transform.parent = null;
		AudioS.Play ();
		StartCoroutine (SelfBreak ());
		transform.FindChild ("Steam").gameObject.SetActive (true);
		transform.FindChild ("Afterburner").gameObject.SetActive (true);
	}

	private void shootReady_E ()
	{
		//audioS.Play();
		StartCoroutine (SelfBreak ());
	}

	private IEnumerator GetAiming (Transform tgt, bool player)
	{
		if(player){
			Vector3 TgtPos = new Vector3 (tgt.transform.position.x, tgt.transform.position.y, tgt.transform.position.z);
			transform.LookAt (TgtPos);
			yield return null;
		}
		try{
		Vector3 TgtPos = new Vector3 (tgt.transform.position.x + Random.Range (-15, 15), tgt.transform.position.y + Random.Range (-15, 15), tgt.transform.position.z + Random.Range (-15, 15));
		transform.LookAt (TgtPos);
		}catch{
		}
		yield return null;
	}

	private IEnumerator MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * Speed);
		yield return null;
	}

	void OnTriggerEnter (Collider col)
	{
		if (transform.parent != null) {
			return;
		}
		StartCoroutine (BreakMissile ());
	}

	private IEnumerator SelfBreak ()
	{
		float time = 0f;
		while (!GameManager.GameOver && time < 20f) {
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (BreakMissile ());
		yield return null;
	}

	private IEnumerator BreakMissile ()
	{
		StopAllCoroutines ();
		Instantiate (Resources.Load ("prefabs/Explosion"), transform.position, Quaternion.identity);
		Destroy (gameObject);
		yield return null;
	}
}

