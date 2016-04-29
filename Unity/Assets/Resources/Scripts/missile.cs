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
		StartCoroutine (GameManager.reloadMissile(startPos,startRot));
		transform.parent = null;
        audioS.Play();
		while (!GameManager.GameOver){
            try
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed);
            }catch{}
			yield return null;
		}
	}

	public void GetAim(Transform tgt)
	{
		transform.LookAt (tgt);
//		Vector3 tgtDis = tgt - transform.position;
//		Vector3.Angle (tgtDis,transform.forward);
	}
	public IEnumerator straight(Transform tgt){
		GetAim (tgt);
		while (!GameManager.GameOver){
			try
			{
				transform.Translate(Vector3.forward * Time.deltaTime * speed);
			}catch{}
			yield return null;
		}
	}

	void OnTriggerEnter(Collider col){
		StartCoroutine (breakMissile());
	}
	private IEnumerator breakMissile(){
		audioS.clip = audioClip2;
		audioS.volume = 0.5f;
		audioS.Play ();
		yield return new WaitForSeconds (3);
		Destroy (gameObject);
	}
}
