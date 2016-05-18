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

	private float Speed;//700
	private float LifeTime = 40;

	private static Airframe AirFrame;

	private Vector3 StartPos;
	private Quaternion StartRot;


	void Awake(){
		AirFrame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		AudioS = gameObject.GetComponent<AudioSource> ();
	}

	void Start ()
	{
		if(gameObject.layer == 12){
		EstimationSystem.Missiles = gameObject;
		}
		AudioS.clip = AudioClip1;
		StartPos = transform.localPosition;
		StartRot = transform.localRotation;
		Speed = 700;//700
	}

	public IEnumerator Straight ()
	{
		ShootReady ();
		while (!GameManager.GameOver) {
			StartCoroutine(MoveForward ());
			yield return null;
		}
	}



	public IEnumerator StraightEnemy (Transform tgt,bool CutOutReady)
	{
		if (!CutOutReady) {
			shootReady_E ();
			transform.LookAt (tgt);
		}
		while (!GameManager.GameOver) {
			StartCoroutine(MoveForward ());
			yield return null;
		}
	}

	public IEnumerator Tracking (Transform tgt)
	{
		ShootReady ();
		while (!GameManager.GameOver) {
			StartCoroutine (GetAimingPlayer (tgt));
			StartCoroutine(MoveForward ());
			yield return null;
		}
		yield return null;
	}

	public IEnumerator TrackingEnemy (Transform tgt)
	{
		shootReady_E ();
		while (!GameManager.GameOver) {
			Vector3 Random3 = new Vector3 (Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));
			//Vector3 TgtPos = GetAimingEnemy (tgt);
//			while (Mathf.Abs(Vector3.Distance(transform.position,TgtPos)) > 200){
//				StartCoroutine(MoveForward ());
//				yield return null;
//			}
			//method
			while(Mathf.Abs(Vector3.Distance(tgt.position,transform.position)) >= 40){
				transform.LookAt (tgt.transform.position + Random3);
				StartCoroutine(MoveForward ());
				yield return null;
			}
			if(Mathf.Abs(Vector3.Distance(tgt.position,transform.position)) < 40){
				Debug.Log ("Swich");
				StopCoroutine (SelfBreak());
				LifeTime = 4;
				StartCoroutine (SelfBreak());
				while (!GameManager.GameOver) {
					StartCoroutine (MoveForward ());
					//StartCoroutine (StraightEnemy(tgt,true));
					yield return null;
				}
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

	private IEnumerator GetAimingPlayer(Transform tgt){
		Vector3 TgtPos = new Vector3 (tgt.transform.position.x, tgt.transform.position.y, tgt.transform.position.z);
		transform.LookAt (TgtPos);
		yield return null;
	}

//	private Vector3 GetAimingEnemy (Transform tgt)
//	{
//		
////		try{
////			Vector3 Distance = new Vector3 ((tgt.transform.position.x+ Random.Range (-40, 40)) - transform.position.x,(tgt.transform.position.y+ Random.Range (-40, 40)) - transform.position.y, (tgt.transform.position.z+ Random.Range (-40, 40)) - transform.position.z);
////			Vector3 TgtPos = new Vector3 (tgt.position.x - Distance.x/2,tgt.position.y - Distance.y/2,tgt.position.z - Distance.z/2);
////		transform.LookAt (TgtPos);
////			return TgtPos;
////		}catch{
////			return Vector3.zero;
////		}
//		Vector3 a;
//		return Vector3;
//	}
//
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
		while (!GameManager.GameOver && time < LifeTime) {
//			if(transform.localPosition.y <= 0){
//				StartCoroutine (BreakMissile ());
//				yield break;
//			}
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (BreakMissile ());
		yield return null;
	}

	private IEnumerator BreakMissile ()
	{
		StopAllCoroutines ();
		EstimationSystem.RemoveList (gameObject);
		Instantiate (Resources.Load ("prefabs/Explosion"), transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.2f);
		Destroy (gameObject);
		yield return null;
	}
}

