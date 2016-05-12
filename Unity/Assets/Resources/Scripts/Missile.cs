using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missile : MonoBehaviour {
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	private AudioSource AudioS;
	public float speed = 850f;//時速3000km
	private static Airframe AirFrame;
	public Vector3 StartPos;
	public Quaternion StartRot;

	void Start () {
		AirFrame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		AudioS = gameObject.GetComponent<AudioSource>();
		AudioS.clip = audioClip1;
		StartPos = transform.localPosition;
		StartRot = transform.localRotation;
	}

	public IEnumerator ShootReady(){
		AirFrame.reload (StartPos,StartRot); //StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
		transform.parent = null;
		AudioS.Play();
		StartCoroutine (SelfBreak ());
		yield return null;
	}

	public IEnumerator shootReady_E(){
		//audioS.Play();
		StartCoroutine (SelfBreak ());
		yield return null;
	}

	public IEnumerator Straight(){
		StartCoroutine (ShootReady ());
		while (!GameManager.GameOver){
            try
            {
				StartCoroutine(MoveForward());
			}catch{
				break;
			}
			yield return null;
		}
	}

	public float Atan (float p2a,float p1a,float p2b,float p1b){
		float d1 = p2a - p1a;
		float d2 = p2b - p1b;
		return (Mathf.Atan2(d1, d2)*Mathf.Rad2Deg);

	}

	public IEnumerator GetAiming(Transform tgt,bool player){
		float Xrad = Atan(tgt.transform.position.y,transform.position.y,tgt.transform.position.z,transform.position.z);
		float Yrad = Atan(tgt.transform.position.x , transform.position.x,tgt.transform.position.z , transform.position.z);
		transform.Rotate(Yrad,Xrad,0,Space.Self);
		yield return null;
	}

	public IEnumerator Straight(Transform tgt){
		StartCoroutine (shootReady_E ());
		transform.LookAt (tgt);
		while (!GameManager.GameOver){
			try
			{
				StartCoroutine(MoveForward());
			}catch{}
			yield return null;
		}
	}

	public IEnumerator Tracking(Transform tgt){
		StartCoroutine (ShootReady ());
		float delay = 0f;
		while (!GameManager.GameOver){
			delay += Time.deltaTime;
			try
			{
				//if(delay >= 0.5f){
				StartCoroutine(GetAiming(tgt,true));
					delay = 0f;
				//}
				StartCoroutine(MoveForward());
			}catch{}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator Tracking_E(Transform tgt){
		StartCoroutine (shootReady_E ());
		transform.LookAt (tgt);
		float delay = 0f;
		while (!GameManager.GameOver){
			delay += Time.deltaTime;
			try
			{
				//if(delay >= 0.5f){
				StartCoroutine(GetAiming(tgt,false));
					delay = 0f;
				//}
				StartCoroutine(MoveForward());
			}catch{}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator MoveForward(){
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
		yield return null;
	}

	void OnTriggerEnter(Collider col){
		if (transform.parent != null) {
			return;
		}
		StartCoroutine (BreakMissile());
	}

	private IEnumerator SelfBreak(){
		float time = 0f;
		while (!GameManager.GameOver && time < 20f) {
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (BreakMissile ());
		yield return null;
	}

	private IEnumerator BreakMissile(){
		Instantiate(Resources.Load("prefabs/Explosion"),transform.position,Quaternion.identity);
		Destroy (gameObject);
		yield return null;
	}
}

