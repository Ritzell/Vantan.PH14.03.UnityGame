using UnityEngine;
using System.Collections;

public class missile : MonoBehaviour {
	public AudioClip audioClip1;
	public AudioClip audioClip2;
	private float speed = 500f;
	private Vector3 startPos;
	private Quaternion startRot;
	public static Transform root;
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
		StartCoroutine (GameManager.reloadMissile(gameObject.name,startPos,startRot));
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
