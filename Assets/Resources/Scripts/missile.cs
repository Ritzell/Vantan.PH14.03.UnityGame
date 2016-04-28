<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

public class missile : MonoBehaviour {
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	private float speed = 500f;
	private Vector3 startPos;
	private Quaternion startRot;
	private static Transform root;
	private AudioSource audioS;

	void Start () {
		audioS = gameObject.GetComponent<AudioSource>();
		audioS.clip = audioClip1;
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		root = GameObject.Find ("missiles").transform;
	}
	
	// Update is called once per frame

	public IEnumerator straight(){
        StartCoroutine (reloadMissile());
		transform.parent = null;
        audioS.Play();
        for (float timer = 0f; timer < 0.1f;timer += Time.deltaTime)
        {
            transform.Translate(0, -0.1f, 0);
            yield return null;
        }
        while (true){
            try
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed);
            }catch{}
			yield return null;
		}
        yield return null;
	}
	
	private IEnumerator reloadMissile(){
		float timer = 0f;
		while (timer < 3f) {
			timer += Time.deltaTime;
			yield return null;
		}
		GameObject newMissile = (GameObject)Instantiate (Resources.Load("prefabs/"+gameObject.name),Vector3.zero,Quaternion.identity);
        newMissile.transform.parent = root;
		newMissile.name = name.Substring (0,8);
		newMissile.transform.localPosition = startPos;
		newMissile.transform.localRotation = startRot;
        yield return new WaitForSeconds(0.35f);
        Attack.missiles.Enqueue(newMissile);
        yield return new WaitForSeconds(10);
		Destroy (gameObject);
		yield return null;
	}
}
=======
﻿using UnityEngine;
using System.Collections;

public class missile : MonoBehaviour {
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	private float speed = 500f;
	private Vector3 startPos;
	private Quaternion startRot;
	private static Transform root;
	private AudioSource audioSource;

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClip1;
		startPos = transform.localPosition;
		startRot = transform.localRotation;
		root = GameObject.Find ("missiles").transform;
	}
	
	// Update is called once per frame

	public IEnumerator straight(){
		audioSource.Play ();
		StartCoroutine (reloadMissile());
		float timer = 0f;
		transform.parent = null;
		while(timer < 0.5f){
			timer += Time.deltaTime;
			yield return null;
		}
		while(!GameOver){
			transform.Translate (Vector3.back * Time.deltaTime * speed);
			yield return null;
		}
	}
	private bool GameOver = false;
	private IEnumerator reloadMissile(){
		float timer = 0f;
		while (timer < 1.8f) {
			timer += Time.deltaTime;
			yield return null;
		}
		GameObject newMissile = (GameObject)Instantiate (Resources.Load("prefabs/"+gameObject.name),Vector3.zero,Quaternion.identity);
		newMissile.transform.parent = root;
		newMissile.name = name.Substring (0,8);
		newMissile.transform.localPosition = startPos;
		newMissile.transform.localRotation = startRot;
		Attack.missiles.Enqueue (newMissile);
		while (timer < 11) {
			timer += Time.deltaTime;
			if (timer >= 10) {
				GameOver = true;
				Destroy (gameObject);
			}
			yield return null;
		}
		yield return null;
	}
}
>>>>>>> 37ffaef15fce39cf566d16ffa856979c0f36a069
