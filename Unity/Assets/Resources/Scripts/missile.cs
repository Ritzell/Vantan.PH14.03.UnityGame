using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class missile : MonoBehaviour {
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	private AudioSource audioS;
	private float speed = 850f;//時速3000km
	private Vector3 startPos;
	private Quaternion startRot;
	public static Transform root;

	void Start () {
		audioS = gameObject.GetComponent<AudioSource>();
		audioS.clip = audioClip1;
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		root = GameObject.Find ("missiles").transform;
	}

	public IEnumerator straight(){
		StartCoroutine (GameObject.Find("GameManager").GetComponent<GameManager>().reloadMissile(startPos,startRot));
		transform.parent = null;
        audioS.Play();
		StartCoroutine (selfBreak ());
		while (!GameManager.GameOver){
            try
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed);
			}catch{
				break;
			}
			yield return null;
		}
	}

	public IEnumerator GetAim(Transform tgt)
	{
		float dx = tgt.transform.position.x - transform.position.x;
		float dy = tgt.transform.position.y - transform.position.y;
		float dz = tgt.transform.position.z - transform.position.z;
		float Yrad = (Mathf.Atan2(dy, dz)*Mathf.Rad2Deg)*-1;
		float Xrad = (Mathf.Atan2 (dx, dz)*Mathf.Rad2Deg)-180;
		transform.rotation = Quaternion.Euler (Yrad,Xrad,0);
		yield return null;
	}

	public IEnumerator GetAiming(Transform tgt){
		float dx = (tgt.transform.position.x - transform.position.x) + Random.Range(-1,1);
		float dy = (tgt.transform.position.y - transform.position.y) + Random.Range(-1,1);
		float dz = (tgt.transform.position.z - transform.position.z) + Random.Range(-1,1);
		float Yrad = (Mathf.Atan2(dy, dz)*Mathf.Rad2Deg)*-1;
		float Xrad = (Mathf.Atan2 (dx, dz)*Mathf.Rad2Deg)-180;
		transform.rotation = Quaternion.Euler (Yrad,Xrad,0);
		yield return null;
	}

	public IEnumerator straight(Transform tgt){
		StartCoroutine(GetAim (tgt));
		StartCoroutine (selfBreak ());
		while (!GameManager.GameOver){
			try
			{
				moveForward();
			}catch{}
			yield return null;
		}
	}

	public IEnumerator Tracking(Transform tgt){
		while (!GameManager.GameOver){
			try
			{
				StartCoroutine(GetAiming(tgt));
				moveForward();
			}catch{}
			yield return null;
		}
		yield return null;
	}

	public void moveForward(){
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
		GameObject soundOb = new GameObject();
		soundOb.AddComponent<explosion>();
		Destroy (gameObject);
		yield return null;
	}
}

