using UnityEngine;
using System.Collections;

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
		StartCoroutine (GameManager.reloadMissile(gameObject.name,startPos,startRot));
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
