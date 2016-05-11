using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class missile : MonoBehaviour {
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	private AudioSource audioS;
	public float speed = 850f;//時速3000km
	private static Airframe airframe;
	public Vector3 startPos;
	public Quaternion startRot;

	void Start () {
		airframe = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		audioS = gameObject.GetComponent<AudioSource>();
		audioS.clip = audioClip1;
		startPos = transform.localPosition;
		startRot = transform.localRotation;
	}

	public IEnumerator shootReady(){
		airframe.reload (startPos,startRot); //StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
		transform.parent = null;
		audioS.Play();
		StartCoroutine (selfBreak ());
		yield return null;
	}

	public IEnumerator shootReady_E(){
		//audioS.Play();
		StartCoroutine (selfBreak ());
		yield return null;
	}

	public IEnumerator straight(){
		StartCoroutine (shootReady ());
		while (!GameManager.GameOver){
            try
            {
				StartCoroutine(moveForward());
			}catch{
				break;
			}
			yield return null;
		}
	}

	public float atan (float p2a,float p1a,float p2b,float p1b){
		float d1 = p2a - p1a;
		float d2 = p2b - p1b;
		return (Mathf.Atan2(d1, d2)*Mathf.Rad2Deg);

	}

	public IEnumerator GetAiming(Transform tgt,bool player){
		float Xrad = atan(tgt.transform.position.y,transform.position.y,tgt.transform.position.z,transform.position.z);
		float Yrad = atan(tgt.transform.position.x , transform.position.x,tgt.transform.position.z , transform.position.z);
		transform.eulerAngles = new Vector3 (Yrad,Xrad,0);
		//transform.Rotate(Xrad,Yrad,0);
//		//transform.rotation = Quaternion.Euler (Yrad*-1, 0, 0);
		yield return null;
	}

	public IEnumerator straight(Transform tgt){
		StartCoroutine (shootReady_E ());
		transform.LookAt (tgt);
		while (!GameManager.GameOver){
			try
			{
				StartCoroutine(moveForward());
			}catch{}
			yield return null;
		}
	}

	public IEnumerator Tracking(Transform tgt){
		StartCoroutine (shootReady ());
		float delay = 0f;
		while (!GameManager.GameOver){
			delay += Time.deltaTime;
			try
			{
				//if(delay >= 0.5f){
				StartCoroutine(GetAiming(tgt,true));
					delay = 0f;
				//}
				StartCoroutine(moveForward());
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
				StartCoroutine(moveForward());
			}catch{}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator moveForward(){
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
		yield return null;
	}

	void OnTriggerEnter(Collider col){
		if (transform.parent != null) {
			return;
		}
		StartCoroutine (breakMissile());
	}

	private IEnumerator selfBreak(){
		float time = 0f;
		while (!GameManager.GameOver && time < 20f) {
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (breakMissile ());
		yield return null;
	}

	private IEnumerator breakMissile(){
		Instantiate(Resources.Load("prefabs/Explosion"),transform.position,Quaternion.identity);
		Destroy (gameObject);
		yield return null;
	}
}

